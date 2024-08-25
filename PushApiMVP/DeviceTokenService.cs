using System.Collections.Concurrent;

namespace PushApiMVP
{
    public interface IDeviceTokenService
    {
        void RegisterToken(string token);
        void UnregisterToken(string token);
        IEnumerable<string> GetAllTokens();
    }

    public class DeviceTokenService : IDeviceTokenService
    {
        private readonly ConcurrentDictionary<string, bool> _tokens = new ConcurrentDictionary<string, bool>();

        public void RegisterToken(string token)
        {
            _tokens.TryAdd(token, true);
        }

        public void UnregisterToken(string token)
        {
            _tokens.TryRemove(token, out _);
        }

        public IEnumerable<string> GetAllTokens()
        {
            return _tokens.Keys;
        }
    }
}