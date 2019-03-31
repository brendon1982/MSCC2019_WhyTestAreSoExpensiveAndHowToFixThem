using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;

namespace Ls.Domain.Tests.Builders
{
    public class SubstituteFsItemPresenterBuilder
    {
        private Action<List<IFsItem>> _fsItemsSnapshot;

        public static SubstituteFsItemPresenterBuilder Create()
        {
            return new SubstituteFsItemPresenterBuilder();
        }

        public SubstituteFsItemPresenterBuilder WithFsItemsSnapshot(Action<List<IFsItem>> fsItemsSnapshot)
        {
            _fsItemsSnapshot = fsItemsSnapshot;

            return this;
        }

        public IFsItemPresenter Build()
        {
            var presenter = Substitute.For<IFsItemPresenter>();
           presenter.Respond(Arg.Do<IEnumerable<IFsItem>>(
               items =>
               {
                   _fsItemsSnapshot(items.ToList());
               })
           );

            return presenter;
        }
    }
}