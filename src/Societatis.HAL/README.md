# Hypermedia HAL
Provides an easy to use wrapper around your models,
in order to provide links and embedded resources.
Meets the [HAL specification](https://tools.ietf.org/html/draft-kelly-json-hal-08).

## Functionality
### Resources
Transform your model to a HAL resource. Through inheritance:
```cs
class Model : Resource
{
    // implementation
}
```
Or using the generic resource:
```cs
var model = new Model();
var resource = new Resource<Model>(model);
```

### Links
Add links to your resources. They are added in a relation to the model.
```cs
var details = new Link("/details");
// Set title, name, hreflang etc. here.
resource.Links["details"].Add(details);
```
Or assign your own collection.
```cs
var orders = new List<ILink>
{
    new Link("/orders/1"),
    new Link("/orders/2"),
    // etc.
};
resource.Links["orders"] = orders;
```
### Curies
### Embedded resources
## Advanced functionality
## Inner working
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

