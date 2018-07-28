namespace CMSCore.Library.Core.Service
{
    using System.Threading.Tasks;
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc;
    using Orleans;
    using Orleans.Configuration.Overrides;

    /// <inheritdoc />
    /// <summary>
    /// Provides information of the current Orleans cluster
    /// </summary>
    public abstract class ClusterControllerBase : Controller
    {
        private readonly IClusterClient _client;

        protected ClusterControllerBase(IClusterClient client)
        {
            _client = client;
        }

        /// <summary>
        ///  Initializes a connection to ClusterClient
        /// </summary>
        /// <returns></returns>
        [HttpGet("connect")]
        public virtual async Task<IActionResult> Connect()
        {
            try
            {
                await _client.Connect();

                return Json(new
                {
                    _client.IsInitialized
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Closes connection to ClusterClient
        /// </summary>
        [HttpGet("close")]
        public virtual async Task<IActionResult> Close()
        {
            try
            {
                await _client.Close();

                return Json(new
                {
                    _client.IsInitialized
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Returns ClusterOptions from ClusterClient.ServiceProvider
        /// </summary>
        /// <param name="providerName">Orleans provider name</param>
        [HttpGet("options/{providerName}")]
        public virtual IActionResult Options([Required] string providerName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("parameter providername is required.");
            }

            try
            {
                var options = _client.ServiceProvider.GetProviderClusterOptions(providerName);
                return Json(options);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}