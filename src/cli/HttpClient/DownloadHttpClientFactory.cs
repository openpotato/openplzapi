#region OpenPLZ API - Copyright (C) 2023 STÜBER SYSTEMS GmbH
/*    
 *    OpenPLZ API 
 *    
 *    Copyright (C) 2023 STÜBER SYSTEMS GmbH
 *
 *    This program is free software: you can redistribute it and/or modify
 *    it under the terms of the GNU Affero General Public License, version 3,
 *    as published by the Free Software Foundation.
 *
 *    This program is distributed in the hope that it will be useful,
 *    but WITHOUT ANY WARRANTY; without even the implied warranty of
 *    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 *    GNU Affero General Public License for more details.
 *
 *    You should have received a copy of the GNU Affero General Public License
 *    along with this program. If not, see <http://www.gnu.org/licenses/>.
 *
 */
#endregion

using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;
using System.Net;
using System.Net.Http;

namespace OpenPlzApi.CLI
{
    public static class DownloadHttpClientFactory
    {
        public static IDownloadHttpClient CreateClient()
        {
            // Create dependency injection container
            var serviceCollection = new ServiceCollection();

            // Register Http Client
            serviceCollection.AddHttpClient<IDownloadHttpClient, DownloadHttpClient>(c =>
            {
                c.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue(AssemblyInfo.GetAgentName(), AssemblyInfo.GetVersion()));
            })

            // Configure HTTP client for automatic retry
            .AddPolicyHandler(Policy<HttpResponseMessage>.Handle<HttpRequestException>()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.RequestTimeout)
                .OrResult(msg => msg.StatusCode == HttpStatusCode.ServiceUnavailable)
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

            // Create IOpenPLZHttpClient implementation
            var services = serviceCollection.BuildServiceProvider();

            // Return back IOpenPLZHttpClient implementation
            return services.GetRequiredService<IDownloadHttpClient>();
        }
    }
}
