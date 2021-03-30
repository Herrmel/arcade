// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.Cci.Extensions;
using Microsoft.Cci.Mappings;

namespace Microsoft.DotNet.AsmDiff.CSV
{
    public abstract class DiffAssemblyCsvColumn : DiffCsvColumn
    {
        private readonly int _index;

        protected DiffAssemblyCsvColumn(DiffConfiguration diffConfiguration, int index)
            : base(diffConfiguration)
        {
            _index = index;
        }

        public override bool IsVisible
        {
            get { return _index == 0 || DiffConfiguration.IsDiff; }
        }

        public override string Name
        {
            get
            {
                return _index switch
                {
                    0 => DiffConfiguration.IsDiff
                        ? "OldAssembly"
                        : "Assembly",
                    1 => "NewAssembly",
                    _ => throw new ArgumentException(),
                };
            }
        }

        public override string GetValue(TypeMapping mapping)
        {
            var assembly = _index < mapping.ElementCount && mapping[_index] != null
                               ? mapping[_index].GetAssembly()
                               : null;
            return assembly == null ? string.Empty : assembly.Name.Value;
        }

        public override string GetValue(MemberMapping mapping)
        {
            var containingTypeDefinition = _index < mapping.ElementCount && mapping[_index] != null
                               ? mapping[_index].ContainingTypeDefinition
                               : null;

            var assembly = containingTypeDefinition?.GetAssembly();

            return assembly == null ? string.Empty : assembly.Name.Value;
        }
    }
}
