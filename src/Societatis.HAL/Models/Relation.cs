// using System;
// using Societatis.Misc;

// namespace Societatis.HAL
// {
//     public class Relation<T> : IRelation<T>, IEquatable<Relation<T>>
//     {
//         public Relation(string rel, T item)
//         {
//             rel.ThrowIfNullOrWhiteSpace(nameof(rel));
//             item.ThrowIfNull(nameof(item));

//             this.Item = item;
//             this.Rel = rel;
//         }

//         public T Item { get; protected set; }
//         public string Rel { get; protected set; }

//         public override bool Equals(object obj)
//         {
//             return this.Equals(obj as Relation<T>);
//         }

//         public bool Equals(Relation<T> other)
//         {
//             bool result = false;
//             if (other != null)
//             {
//                 result = this.Rel.Equals(other.Rel)
//                     && this.Item.Equals(other.Item);
//             }

//             return result;
//         }

//         public override int GetHashCode()
//         {

//         }
//     }
// }