using System;

namespace IIASA.FloodCitiSense.Mobile.Core.Core.Entity
{
    /// <summary>
    /// </summary>
    public class AppUser
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AppUser" /> class.
        /// </summary>
        public AppUser()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AppUser" /> class.
        /// </summary>
        /// <param name="id">
        ///     The id.
        /// </param>
        /// <param name="token">
        ///     The token.
        /// </param>
        /// <param name="firstName">
        ///     The first name.
        /// </param>
        /// <param name="lastName">
        ///     The last name.
        /// </param>
        /// <param name="email">
        ///     The email.
        /// </param>
        /// <param name="imageUrl">
        ///     The image url.
        /// </param>
        /// <param name="provider"></param>
        public AppUser(string id, string token, string firstName, string lastName, string email, string imageUrl, string provider)
        {
            this.Id = id;
            this.Token = token;
            this.Name = firstName + lastName;
            this.Email = email;
            this.Picture = new Uri(imageUrl);
            this.Provider = provider;
        }

        /// <summary>
        ///     Gets or sets the email.
        /// </summary>
        /// <value>
        ///     The email.
        /// </value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the picture.
        /// </summary>
        /// <value>
        ///     The picture.
        /// </value>
        public Uri Picture { get; set; }

        /// <summary>
        ///     Gets or sets the token.
        /// </summary>
        public string Token { get; set; }

        public string Provider { get; set; }
    }
}