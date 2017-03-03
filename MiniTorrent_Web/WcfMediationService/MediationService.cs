using System;
using MiniTorrent_MediationServerContract;

namespace WcfMediationService
{
    public class MediationService : IMediationServerContract
    {
        public string GetName()
        {
            return "Name1";
        }
    }
}
