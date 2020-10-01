using Abp;

namespace IIASA.FloodCitiSense.CustomException
{
    public class TenantNotAvailableException : AbpException
    {
        public TenantNotAvailableException() : base()
        {
        }

        public TenantNotAvailableException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext context) : base(serializationInfo, context)
        {
        }

        public TenantNotAvailableException(string message) : base(message)
        {
        }

        public TenantNotAvailableException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}