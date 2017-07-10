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

        public override bool? Templated
        {
            get => base.Templated;
            set
            {
                if (!value.HasValue 
                || value == false)
                {
                    throw new InvalidOperationException("A curie is always templated.");
                }

                base.Templated = value;
            }
        }

        public static bool IsCurie(ILink link)
        {
            link.ThrowIfNull(nameof(link));
            bool result;

            if (link is Curie)
            {
                result = true;
            }
            // TODO: Improve this method to use the curie specification
            else if (link.Templated == true
                && link.Name != null
                && link.HRef.ToString().Contains("{rel}"))
            {
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }
    }
}