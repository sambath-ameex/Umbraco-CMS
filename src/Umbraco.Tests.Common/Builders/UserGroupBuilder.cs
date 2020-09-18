using System.Collections.Generic;
using System.Linq;
using Moq;
using Umbraco.Core.Models.Membership;
using Umbraco.Core.Strings;
using Umbraco.Tests.Common.Builders.Interfaces;

namespace Umbraco.Tests.Common.Builders
{
    public class UserGroupBuilder : UserGroupBuilder<object>
    {
        public UserGroupBuilder() : base(null)
        {
        }
    }

    public class UserGroupBuilder<TParent>
        : ChildBuilderBase<TParent, IUserGroup>,
            IWithIdBuilder,
            IWithIconBuilder,
            IWithAliasBuilder,
            IWithNameBuilder
    {
        private int? _id;
        private string _alias;
        private string _icon;
        private string _name;
        private IEnumerable<string> _permissions = Enumerable.Empty<string>();
        private IEnumerable<string> _sectionCollection = Enumerable.Empty<string>();
        private string _suffix;
        private int? _startContentId;
        private int? _startMediaId;
        private int? _userCount;

        public UserGroupBuilder(TParent parentBuilder) : base(parentBuilder)
        {
        }

        /// <summary>
        /// Will suffix the name and alias for testing
        /// </summary>
        /// <param name="suffix"></param>
        /// <returns></returns>
        public UserGroupBuilder<TParent> WithSuffix(string suffix)
        {
            _suffix = suffix;
            return this;
        }

        public UserGroupBuilder<TParent> WithUserCount(int userCount)
        {
            _userCount = userCount;
            return this;
        }

        public UserGroupBuilder<TParent> WithPermissions(string permissions)
        {
            _permissions = permissions.Split();
            return this;
        }

        public UserGroupBuilder<TParent> WithPermissions(IList<string> permissions)
        {
            _permissions = permissions;
            return this;
        }

        public UserGroupBuilder<TParent> WithStartContentId(int startContentId)
        {
            _startContentId = startContentId;
            return this;
        }

        public UserGroupBuilder<TParent> WithStartMediaId(int startMediaId)
        {
            _startMediaId = startMediaId;
            return this;
        }

        public IReadOnlyUserGroup BuildReadOnly(IUserGroup userGroup)
        {
            return Mock.Of<IReadOnlyUserGroup>(x =>
                x.Permissions == userGroup.Permissions &&
                x.Alias == userGroup.Alias &&
                x.Icon == userGroup.Icon &&
                x.Name == userGroup.Name &&
                x.StartContentId == userGroup.StartContentId &&
                x.StartMediaId == userGroup.StartMediaId &&
                x.AllowedSections == userGroup.AllowedSections &&
                x.Id == userGroup.Id);
        }

        public override IUserGroup Build()
        {
            var id = _id ?? 0;
            var name = _name ?? ("TestUserGroup" + _suffix);
            var alias = _alias ?? ("testUserGroup" + _suffix);
            var userCount = _userCount ?? 0;
            var startContentId = _startContentId ?? -1;
            var startMediaId = _startMediaId ?? -1;
            var icon = _icon ?? "icon-group";

            var shortStringHelper = new DefaultShortStringHelper(new DefaultShortStringHelperConfig());

            var userGroup = new UserGroup(shortStringHelper, userCount, alias, name, _permissions, icon);
            userGroup.Id = id;
            userGroup.StartContentId = startContentId;
            userGroup.StartMediaId = startMediaId;

            foreach (var item in _sectionCollection)
            {
                userGroup.AddAllowedSection(item);
            }

            return userGroup;
        }

       int? IWithIdBuilder.Id
        {
            get => _id;
            set => _id = value;
        }

        string IWithIconBuilder.Icon
        {
            get => _icon;
            set => _icon = value;
        }

        string IWithAliasBuilder.Alias
        {
            get => _alias;
            set => _alias = value;
        }

        string IWithNameBuilder.Name
        {
            get => _name;
            set => _name = value;
        }
    }
}