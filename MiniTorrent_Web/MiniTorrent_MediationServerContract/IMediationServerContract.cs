using System.ServiceModel;

namespace MiniTorrent_MediationServerContract
{
    [ServiceContract]
    public interface IMediationServerContract
    {
        [OperationContract]
        string GetName();
    }
}
