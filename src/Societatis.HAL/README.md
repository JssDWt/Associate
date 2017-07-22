
Include type information of the resource (inherited type) or T.

### Serialization

### Deserialization
Resource and Resource<T>
- The root type is expected to be a concrete type.
- Its links will be populated with a type found on the json contract. 
  - If it can't find a concrete type, The type set on the JsonConverter property is used. The property contains a concrete type with a default constructor, and implements the interface of a link collection. The link type is also concrete.
  - If that is not a match, an Exception is thrown.
- Its embedded resources are slightly more difficult. Each relation could have its own type. So there are a few options:
  1. Type information from the json resource itself is used to find the right class.
  2. The JsonConverter will have a set of functions that provide the right type for a resource. Provided in the function will be the resource type and its relation. The first function that returns a concrete type is used.
  3. If ThrowOnTypeNotFound is true, an exception is raised, otherwise, only the resource information will be deserialized.
