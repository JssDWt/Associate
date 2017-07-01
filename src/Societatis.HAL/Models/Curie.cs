namespace Societatis.HAL
{
    using System;
    using Societatis.Misc;

    public class Curie : Link
    {
        public const string Relation = "curies";
        public Curie(string name, Uri template)
            : base(template)
        {
            name.ThrowIfNullOrWhiteSpace(nameof(name));
            template.ThrowIfNull(nameof(template));

            this.Templated = true;
            this.Name = name;
        }

        public override bool Templated
        {
            get => base.Templated;
            set
            {
                if (value == false)
                {
                    throw new InvalidOperationException("A curie is always templated.");
                }
            }
        }
    }
}