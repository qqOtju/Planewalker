using System;
using System.Threading.Tasks;

namespace Game.Scripts.Infrastructure.AppBootstrap.Operations
{
    public interface ILoadingOperation
    {
        public Task UseOperation(Action<float> onProgress);
    }
}