using System;
using System.Collections.Generic;
using System.Text;

namespace JustSellIt.Application.Interfaces
{
    public interface IStatusService
    {
        int GetIdBeforeNew();
        int GetIdDraft();
        int GetIdForVeryfication();
        int GetIdPublished();
        int GetIdRejected();
        int GetIdDeleted();
    }
}
