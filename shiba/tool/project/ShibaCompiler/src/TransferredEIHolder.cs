using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// 伝達されたEvaluateInfoを持つクラス。
    /// </summary>
    class TransferredEIHolder
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public TransferredEIHolder(EvaluateInfo aNodeEI)
        {
            mNodeEI = aNodeEI;
            mIsReceived = false;
        }

        //------------------------------------------------------------
        // 伝達されたEIを受け取り、NodeEIのSRを設定する。
        public void ReceiveAndSetSR(SemanticAnalyzeComponent aComp)
        {
            // 伝達されたEvaluateInfoを取得
            mTransferredEI = aComp.TransferredEvaluateInfoReceive();

            // レジスタ確保　
            mIsReceived = isNeedToReserveSR();
            mNodeEI.SR = mIsReceived
                ? aComp.SRReserve()
                : mTransferredEI.SR;
        }

        //------------------------------------------------------------
        // 下層に伝達できるなら伝達する。
        public void TransferIfPossible(SemanticAnalyzeComponent aComp)
        {
            // 可能ならEvaluateInfoを伝達
            if (mIsReceived //自分で確保した
                || mTransferredEI.IsReusableSR // 再利用していいレジスタなので伝達してよい
                )
            {
                aComp.TransferredEvaluateInfoSet(mNodeEI);
            }
        }

        //------------------------------------------------------------
        // レジスタを返却する必要があるなら返却する。
        public void ReleaseIfNeccesary(SemanticAnalyzeComponent aComp)
        {
            if (mIsReceived)
            {
                aComp.SRRelease(mNodeEI.SR);
            }
        }

        //============================================================
        EvaluateInfo mNodeEI;
        EvaluateInfo mTransferredEI;
        bool mIsReceived;

        //------------------------------------------------------------
        // 自分でSRを確保する必要があるか。
        bool isNeedToReserveSR()
        {
            return mTransferredEI == null;  // null ということは自分でレジスタを確保しなければならない。
        }

    }
}
