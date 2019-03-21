using System.Collections.Generic;

namespace Ls.Domain.Tests
{
    public interface IPresenter
    {
        void Respond(IEnumerable<IFsItem> output);
    }
}