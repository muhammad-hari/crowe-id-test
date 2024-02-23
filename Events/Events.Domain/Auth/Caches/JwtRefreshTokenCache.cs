using Events.Domain.Auth.Interfaces;
using Microsoft.Extensions.Hosting;

namespace Events.Domain.Auth.Caches
{
    public class JwtRefreshTokenCache : IHostedService, IDisposable
    {
        #region Private Members

        private Timer timer = default!;
        private readonly IJwtAuthentication jwtAuthentication;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="jwtAuthManager"></param>
        public JwtRefreshTokenCache(IJwtAuthentication jwtAuthManager) => jwtAuthentication = jwtAuthManager;

        #endregion

        public void Dispose()
        {
            timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // remove expired refresh tokens from cache every minute
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1.0));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }


        #region Private Members

        private void DoWork(object state)
        {
            // remove expired token right now.
            jwtAuthentication.RemoveExpiredRefreshTokens(DateTime.Now);
        }

        #endregion
    }
}
