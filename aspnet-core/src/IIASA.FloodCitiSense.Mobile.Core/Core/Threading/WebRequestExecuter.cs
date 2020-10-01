// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebRequestExecuter.cs" company="IIASA">
//   EOS
// </copyright>
// <summary>
//   Defines the WebRequestExecuter.cs type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Abp.Runtime.Validation;
using Abp.UI;
using Acr.UserDialogs;
using Flurl.Http;
using IIASA.FloodCitiSense.CustomException;
using IIASA.FloodCitiSense.Extensions;
using IIASA.FloodCitiSense.Mobile.Core.Core.Dependency;
using IIASA.FloodCitiSense.Mobile.Core.Extensions.MarkupExtensions;
using IIASA.FloodCitiSense.Mobile.Core.Interface;
using IIASA.FloodCitiSense.Mobile.Core.Properties;
using Microsoft.AppCenter.Crashes;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace IIASA.FloodCitiSense.Mobile.Core.Core.Threading
{
    /// <summary>
    /// Defines the <see cref="WebRequestExecuter" />
    /// </summary>
    public static class WebRequestExecuter
    {
        /// <summary>
        /// The Execute
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func">The func<see cref="Func{Task{TResult}}"/></param>
        /// <param name="successCallback">The successCallback<see cref="Func{TResult, Task}"/></param>
        /// <param name="failCallback">The failCallback<see cref="Exception"/></param>
        /// <param name="finallyCallback">The finallyCallback<see cref="Action"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public static async Task Execute<TResult>(
            Func<Task<TResult>> func,
            Func<TResult, Task> successCallback,
            Func<System.Exception, Task> failCallback = null,
            Action finallyCallback = null)
        {
            if (successCallback == null)
            {
                successCallback = _ => Task.CompletedTask;
            }

            if (failCallback == null)
            {
                failCallback = _ => Task.CompletedTask;
            }

            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    UserDialogs.Instance.HideLoading();

                    var accepted = await UserDialogs.Instance.ConfirmAsync(Resources.NoInternet,
                        Resources.MessageTitle, Resources.Ok, Resources.Cancel);

                    if (accepted)
                    {
                        await Execute(func, successCallback, failCallback);
                    }
                    else
                    {
                        await failCallback(new System.Exception(Resources.NoInternet));
                    }
                }
                else
                {
                    await successCallback(await func());
                }
            }
            catch (System.Exception exception)
            {
                Crashes.TrackError(exception);
                await HandleException(exception, func, successCallback, failCallback);
            }
            finally
            {
                finallyCallback?.Invoke();
            }
        }

        /// <summary>
        /// The Execute
        /// </summary>
        /// <param name="func">The func<see cref="Func{Task}"/></param>
        /// <param name="successCallback">The successCallback<see cref="Func{Task}"/></param>
        /// <param name="failCallback">The failCallback<see cref="Exception"/></param>
        /// <param name="finallyCallback">The finallyCallback<see cref="Action"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public static async Task Execute(
            Func<Task> func,
            Func<Task> successCallback = null,
            Func<System.Exception, Task> failCallback = null,
            Action finallyCallback = null)
        {
            if (successCallback == null)
            {
                successCallback = () => Task.CompletedTask;
            }

            if (failCallback == null)
            {
                failCallback = _ => Task.CompletedTask;
            }

            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    UserDialogs.Instance.HideLoading();

                    var accepted = await UserDialogs.Instance.ConfirmAsync(Resources.NoInternet,
                        Resources.MessageTitle, Resources.Ok, Resources.Cancel);

                    if (accepted)
                    {
                        await Execute(func, successCallback, failCallback);
                    }
                    else
                    {
                        await failCallback(new System.Exception(Resources.NoInternet));
                    }
                }
                else
                {
                    await func();
                    await successCallback();
                }
            }
            catch (System.Exception ex)
            {
                Crashes.TrackError(ex);
                await HandleException(ex, func, successCallback, failCallback);
            }
            finally
            {
                finallyCallback?.Invoke();
            }
        }

        /// <summary>
        /// The HandleException
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="exception">The exception<see cref="System.Exception"/></param>
        /// <param name="func">The func<see cref="Func{Task{TResult}}"/></param>
        /// <param name="successCallback">The successCallback<see cref="Func{TResult, Task}"/></param>
        /// <param name="failCallback">The failCallback<see cref="Exception"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private static async Task HandleException<TResult>(System.Exception exception,
            Func<Task<TResult>> func,
            Func<TResult, Task> successCallback,
            Func<System.Exception, Task> failCallback)
        {
            UserDialogs.Instance.HideLoading();
            switch (exception)
            {
                case UserFriendlyException userFriendlyException:
                    await HandleUserFriendlyException(userFriendlyException, failCallback);
                    break;
                case FlurlHttpTimeoutException httpTimeoutException:
                    HandleFlurlHttpTimeoutException(httpTimeoutException, func, successCallback, failCallback);
                    break;
                case FlurlHttpException httpException:
                    await HandleFlurlHttpException(httpException, func, successCallback, failCallback);
                    break;
                case AbpValidationException abpValidationException:
                    await HandleAbpValidationException(abpValidationException, failCallback);
                    break;
                case UnauthorizedAccessException unauthorizedAccessException:
                    await HandleUnauthorizedAccessExceptionAsync(unauthorizedAccessException, failCallback);
                    break;
                case TenantNotAvailableException tenantNotAvailableException:
                    await TenantNotAvailableExceptionAsync(tenantNotAvailableException, failCallback);
                    break;
                default:
                    HandleDefaultException(exception, func, successCallback, failCallback);
                    break;
            }
        }

        private static async Task TenantNotAvailableExceptionAsync(TenantNotAvailableException tenantNotAvailableException, Func<System.Exception, Task> failCallback)
        {
            var nav = Resolver.Resolve<INavigationHelper>();
            await nav.HandleTenantNotAvailableException();
            UserDialogs.Instance.Toast("NotAbleToFindCity".Translate(), TimeSpan.FromSeconds(10));
            await failCallback(tenantNotAvailableException);
        }

        private static async Task HandleUnauthorizedAccessExceptionAsync(UnauthorizedAccessException unauthorizedAccessException, Func<System.Exception, Task> failCallback)
        {
            UserDialogs.Instance.Toast("OperationFailedLogin".Translate(), TimeSpan.FromSeconds(10));
            await failCallback(unauthorizedAccessException);
        }

        /// <summary>
        /// The HandleException
        /// </summary>
        /// <param name="exception">The exception<see cref="System.Exception"/></param>
        /// <param name="func">The func<see cref="Func{Task}"/></param>
        /// <param name="successCallback">The successCallback<see cref="Func{Task}"/></param>
        /// <param name="failCallback">The failCallback<see cref="Exception"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private static async Task HandleException(System.Exception exception,
            Func<Task> func,
            Func<Task> successCallback,
            Func<System.Exception, Task> failCallback)
        {
            UserDialogs.Instance.HideLoading();
            switch (exception)
            {
                case UserFriendlyException userFriendlyException:
                    await HandleUserFriendlyException(userFriendlyException, failCallback);
                    break;
                case FlurlHttpTimeoutException httpTimeoutException:
                    HandleFlurlHttpTimeoutException(httpTimeoutException, func, successCallback, failCallback);
                    break;
                case FlurlHttpException httpException:
                    await HandleFlurlHttpException(httpException, func, successCallback, failCallback);
                    break;
                case AbpValidationException abpValidationException:
                    await HandleAbpValidationException(abpValidationException, failCallback);
                    break;
                case UnauthorizedAccessException unauthorizedAccessException:
                    await HandleUnauthorizedAccessExceptionAsync(unauthorizedAccessException, failCallback);
                    break;
                default:
                    HandleDefaultException(exception, func, successCallback, failCallback);
                    break;
            }
        }

        /// <summary>
        /// The HandleUserFriendlyException
        /// </summary>
        /// <param name="userFriendlyException">The userFriendlyException<see cref="UserFriendlyException"/></param>
        /// <param name="failCallback">The failCallback<see cref="Exception"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private static async Task HandleUserFriendlyException(UserFriendlyException userFriendlyException,
            Func<System.Exception, Task> failCallback)
        {
            await UserDialogs.Instance.AlertAsync(string.IsNullOrEmpty(userFriendlyException.Details)
                ? userFriendlyException.Message.Translate()
                : userFriendlyException.Details);

            await failCallback(userFriendlyException);
        }

        /// <summary>
        /// The HandleFlurlHttpTimeoutException
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="httpTimeoutException">The httpTimeoutException<see cref="FlurlHttpTimeoutException"/></param>
        /// <param name="func">The func<see cref="Func{Task{TResult}}"/></param>
        /// <param name="successCallback">The successCallback<see cref="Func{TResult, Task}"/></param>
        /// <param name="failCallback">The failCallback<see cref="Exception"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private static void HandleFlurlHttpTimeoutException<TResult>(
            FlurlHttpTimeoutException httpTimeoutException,
            Func<Task<TResult>> func,
            Func<TResult, Task> successCallback,
            Func<System.Exception, Task> failCallback)
        {
            UserDialogs.Instance.Toast(Resources.RequestTimedOut, TimeSpan.FromSeconds(10));
            //var accepted = await UserDialogs.Instance.ConfirmAsync(LocalTranslation.RequestTimedOut,
            //    LocalTranslation.MessageTitle, LocalTranslation.Ok, LocalTranslation.Cancel);

            //if (accepted)
            //{
            //    await Execute(func, successCallback, failCallback);
            //}
            //else
            //{
            //    await failCallback(httpTimeoutException);
            //}
        }

        /// <summary>
        /// The HandleFlurlHttpTimeoutException
        /// </summary>
        /// <param name="httpTimeoutException">The httpTimeoutException<see cref="FlurlHttpTimeoutException"/></param>
        /// <param name="func">The func<see cref="Func{Task}"/></param>
        /// <param name="successCallback">The successCallback<see cref="Func{Task}"/></param>
        /// <param name="failCallback">The failCallback<see cref="Exception"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private static void HandleFlurlHttpTimeoutException(FlurlHttpTimeoutException httpTimeoutException,
            Func<Task> func,
            Func<Task> successCallback,
            Func<System.Exception, Task> failCallback)
        {
            UserDialogs.Instance.Toast(Resources.RequestTimedOut, TimeSpan.FromSeconds(10));
            //var accepted = await UserDialogs.Instance.ConfirmAsync(LocalTranslation.RequestTimedOut,
            //    LocalTranslation.MessageTitle, LocalTranslation.Ok, LocalTranslation.Cancel);

            //if (accepted)
            //{
            //    await Execute(func, successCallback, failCallback);
            //}
            //else
            //{
            //    await failCallback(httpTimeoutException);
            //}
        }

        /// <summary>
        /// The HandleFlurlHttpException
        /// </summary>
        /// <param name="httpException">The httpException<see cref="FlurlHttpException"/></param>
        /// <param name="func">The func<see cref="Func{Task}"/></param>
        /// <param name="successCallback">The successCallback<see cref="Func{Task}"/></param>
        /// <param name="failCallback">The failCallback<see cref="Exception"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private static async Task HandleFlurlHttpException(FlurlHttpException httpException,
            Func<Task> func,
            Func<Task> successCallback,
            Func<System.Exception, Task> failCallback)
        {
            if (await AbpExceptionHandler.HandleIfAbpResponseAsync(httpException))
            {
                await failCallback(httpException);
                return;
            }

            //UserDialogs.Instance.Toast(Resources.HttpException, TimeSpan.FromSeconds(10));
            //var accepted = await UserDialogs.Instance.ConfirmAsync(LocalTranslation.HttpException,
            //    LocalTranslation.MessageTitle, LocalTranslation.Ok, LocalTranslation.Cancel);

            //if (accepted)
            //{
            //    await Execute(func, successCallback, failCallback);
            //}
            //else
            //{
            //    await failCallback(httpException);
            //}
        }

        /// <summary>
        /// The HandleFlurlHttpException
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="httpException">The httpException<see cref="FlurlHttpException"/></param>
        /// <param name="func">The func<see cref="Func{Task{TResult}}"/></param>
        /// <param name="successCallback">The successCallback<see cref="Func{TResult, Task}"/></param>
        /// <param name="failCallback">The failCallback<see cref="Exception"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private static async Task HandleFlurlHttpException<TResult>(FlurlHttpException httpException,
            Func<Task<TResult>> func,
            Func<TResult, Task> successCallback,
            Func<System.Exception, Task> failCallback)
        {
            if (await AbpExceptionHandler.HandleIfAbpResponseAsync(httpException))
            {
                await failCallback(httpException);
                return;
            }

            //UserDialogs.Instance.Toast(Resources.HttpException, TimeSpan.FromSeconds(10));
            //var accepted = await UserDialogs.Instance.ConfirmAsync(LocalTranslation.HttpException,
            //    LocalTranslation.MessageTitle, LocalTranslation.Ok, LocalTranslation.Cancel);

            //if (accepted)
            //{
            //    await Execute(func, successCallback, failCallback);
            //}
            //else
            //{
            //    await failCallback(httpException);
            //}
        }

        /// <summary>
        /// The HandleAbpValidationException
        /// </summary>
        /// <param name="abpValidationException">The abpValidationException<see cref="AbpValidationException"/></param>
        /// <param name="failCallback">The failCallback<see cref="Exception"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private static async Task HandleAbpValidationException(AbpValidationException abpValidationException,
            Func<System.Exception, Task> failCallback)
        {
            UserDialogs.Instance.Toast(abpValidationException.GetConsolidatedMessage(), TimeSpan.FromSeconds(10));

            await failCallback(abpValidationException);
        }

        /// <summary>
        /// The HandleDefaultException
        /// </summary>
        /// <param name="exception">The exception<see cref="System.Exception"/></param>
        /// <param name="func">The func<see cref="Func{Task}"/></param>
        /// <param name="successCallback">The successCallback<see cref="Func{Task}"/></param>
        /// <param name="failCallback">The failCallback<see cref="Exception"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private static void HandleDefaultException(System.Exception exception,
            Func<Task> func,
            Func<Task> successCallback,
            Func<System.Exception, Task> failCallback)
        {

            //UserDialogs.Instance.Toast(Resources.UnhandledWebRequestException, TimeSpan.FromSeconds(10));
            //var accepted = await UserDialogs.Instance.ConfirmAsync(LocalTranslation.UnhandledWebRequestException,
            //    LocalTranslation.MessageTitle, LocalTranslation.Ok, LocalTranslation.Cancel);
            //if (accepted)
            //{
            //    await Execute(func, successCallback, failCallback);
            //}
            //else
            //{
            //    await failCallback(exception);
            //}
        }

        /// <summary>
        /// The HandleDefaultException
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="exception">The exception<see cref="System.Exception"/></param>
        /// <param name="func">The func<see cref="Func{Task{TResult}}"/></param>
        /// <param name="successCallback">The successCallback<see cref="Func{TResult, Task}"/></param>
        /// <param name="failCallback">The failCallback<see cref="Exception"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private static void HandleDefaultException<TResult>(System.Exception exception,
            Func<Task<TResult>> func,
            Func<TResult, Task> successCallback,
            Func<System.Exception, Task> failCallback)
        {

            //UserDialogs.Instance.Toast(Resources.UnhandledWebRequestException, TimeSpan.FromSeconds(10));

            //var accepted = await UserDialogs.Instance.ConfirmAsync(LocalTranslation.UnhandledWebRequestException,
            //    LocalTranslation.MessageTitle, LocalTranslation.Ok, LocalTranslation.Cancel);

            //if (accepted)
            //{
            //    await Execute(func, successCallback, failCallback);
            //}
            //else
            //{
            //    await failCallback(exception);
            //}
        }
    }
}
