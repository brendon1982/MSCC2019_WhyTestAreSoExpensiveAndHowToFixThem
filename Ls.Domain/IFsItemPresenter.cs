using System.Collections.Generic;

namespace Ls.Domain
{
    public interface IFsItemPresenter
    {
        void Respond(IEnumerable<IFsItem> output, string error);
    }
}