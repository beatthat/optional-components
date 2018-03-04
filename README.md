
## Usage

```c#
using BeatThat.OptionalComponents

[OptionalComponent]
public class Foo
{
  [Tooltip("set TRUE to disable the behaviour that checks/ensures sibling components defined by the [OptionalComponent] attribute.")]
  public bool m_disableEnsureOptionalComponentsOnStart;

  virtual protected void Start()
  {
    if (!m_disableEnsureOptionalComponentsOnStart) {
      this.EnsureAllOptionalComponents (); // extension function for Component
    }
  }
}
```

### Motivating example: Properties classes

The components in beatthat/state-controller-properties allow you to define components to wrap individual Unity Animator properties. A very common practice is to them bind a property to some authoritative source of the value (this is roughly drawn from common practice in MVVM/Model View View Model frameworks).

Below is an example how you might use this for illustration:

```c#
[OptionalComponent(typeof(BindIsLoggedInToModel))]
public class IsLoggedIn : BoolStateProperty {} // wrap an animator param 'isLoggedIn'

/// binds to events from LoginModel
/// and makes sure it's sibling IsLoggedIn always has accurate value set
public class BindIsLoggedInToModel  {
  ...
}
```

Not seen in the user code above is that somewhere in the base class[es] of BoolStateProperty is a setup that provides the toggle property ```m_disableEnsureOptionalComponentsOnStart```
and the call to

```c#
void Start()
{
  if (!m_disableEnsureOptionalComponentsOnStart) {
    this.EnsureAllOptionalComponents (); // extension function for Component
  }
}
```

...as had been described previously. The resulting behaviour is that having the attribute ```[OptionalComponent(typeof(BindIsLoggedInToModel))]``` the property IsLoggedIn adds its own sibling of type BindIsLoggedInToModel on Start ***unless*** the toggle property ```m_disableEnsureOptionalComponentsOnStart``` is set TRUE on a particular instance.
