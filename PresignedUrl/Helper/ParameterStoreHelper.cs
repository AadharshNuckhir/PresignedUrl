using Amazon;
using Amazon.S3.Model.Internal.MarshallTransformations;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;

namespace PresignedUrl.Helper
{
    public static class ParameterStoreHelper
    {
        public static string GetParameterFromParameterStore(string parameterName)
        {
			try
			{
				var getParameterRequest = new GetParameterRequest()
				{
					Name = parameterName,
					WithDecryption = true
				};

				var maxErrorRetry = Environment.GetEnvironmentVariable("AWS_MAX_ATTEMPTS");

				var parameterStoreConfig = new AmazonSimpleSystemsManagementConfig()
				{
					MaxErrorRetry = int.Parse(maxErrorRetry),
					Timeout = TimeSpan.FromSeconds(10),
					RetryMode = Amazon.Runtime.RequestRetryMode.Adaptive,
					RegionEndpoint = RegionEndpoint.EUCentral1
				};

				GetParameterResponse response;

#if DEBUG
				AmazonSimpleSystemsManagementClient _parameterStoreClient = new(ConfigHelper.AWS_ACCESS_KEY_ID, ConfigHelper.AWS_SECRET_ACCESS_KEY,
																				parameterStoreConfig);
#else
                AmazonSimpleSystemsManagementClient _parameterStoreClient = new(parameterStoreConfig);
#endif
				response = _parameterStoreClient.GetParameterAsync(getParameterRequest).Result;
				var parameter = response.Parameter.Value;

				return parameter;
            }
            catch (Exception)
			{
				throw;
			}
        }

		public static Dictionary<string, string> GetParametersFromParameterStore(List<string> parameterNames)
		{
			try
			{
				var getParametersRequest = new GetParametersRequest()
				{
					Names = parameterNames,
					WithDecryption = true
				};

                var maxErrorRetry = Environment.GetEnvironmentVariable("AWS_MAX_ATTEMPTS");

                var parameterStoreConfig = new AmazonSimpleSystemsManagementConfig()
                {
                    MaxErrorRetry = int.Parse(maxErrorRetry),
                    Timeout = TimeSpan.FromSeconds(10),
                    RetryMode = Amazon.Runtime.RequestRetryMode.Adaptive,
                    RegionEndpoint = RegionEndpoint.EUCentral1
                };

                GetParametersResponse response;

#if DEBUG
                AmazonSimpleSystemsManagementClient _parameterStoreClient = new(ConfigHelper.AWS_ACCESS_KEY_ID, ConfigHelper.AWS_SECRET_ACCESS_KEY,
                                                                                 parameterStoreConfig);
#else
                AmazonSimpleSystemsManagementClient _parameterStoreClient = new(parameterStoreConfig);
#endif
                response = _parameterStoreClient.GetParametersAsync(getParametersRequest).Result;
				var parameters = response.Parameters;

				Dictionary<string, string> dicParameters = new Dictionary<string, string>();
                response.Parameters.ForEach(p => dicParameters.Add(p.Name, p.Value));
					
                return dicParameters;

            }
			catch(Exception ex) 
			{
				throw;
			}
		}

		public static Dictionary<string, string> GetParametersByPathFromParameterStore(string parameterPath)
		{
			try
			{
				var pathRequest = new GetParametersByPathRequest()
				{
					Path = parameterPath,
					WithDecryption = true
				};

				var maxErrorRetry = Environment.GetEnvironmentVariable("AWS_MAX_ATTEMPTS");

                var parameterStoreConfig = new AmazonSimpleSystemsManagementConfig()
                {
                    MaxErrorRetry = int.Parse(maxErrorRetry),
                    Timeout = TimeSpan.FromSeconds(10),
                    RetryMode = Amazon.Runtime.RequestRetryMode.Adaptive,
                    RegionEndpoint = RegionEndpoint.EUCentral1
                };

                GetParametersByPathResponse response;

#if DEBUG
                AmazonSimpleSystemsManagementClient _parameterStoreClient = new(ConfigHelper.AWS_ACCESS_KEY_ID, ConfigHelper.AWS_SECRET_ACCESS_KEY,
                                                                                parameterStoreConfig);
#else
                AmazonSimpleSystemsManagementClient _parameterStoreClient = new(parameterStoreConfig);
#endif
                response = _parameterStoreClient.GetParametersByPathAsync(pathRequest).Result;
                var parameters = response.Parameters;

                Dictionary<string, string> dicParameters = new Dictionary<string, string>();
                response.Parameters.ForEach(p => dicParameters.Add(p.Name, p.Value));

                return dicParameters;
            }
			catch (Exception)
			{
				throw;
			}
		}
    }
}
