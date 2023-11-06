
using System.Threading.Tasks;

namespace Itequia.SpeedCode.Messaging.Interfaces
{
    public interface IMessageHandler
    {
        Task ProcessMessage(string queue, string message);
    }
}
