# Getting started

Testing Blazor components is **_not_** the same as testing a regular class. It basically boils down to that Blazor components are not instantiated directly by you, using the `new` keyword, they are instead rendered, and their output is not directly available.

Thus, for first time Blazor component testers, the recommended reading is (5-10 minutes):

- [Basics of Blazor component testing](/docs/Basics-of-Blazor-component-testing.html)
- [Creating a new test project](/docs/Creating-a-new-test-project.html)

If you prefer a **video based tutorial**, check out:

- [Testing Blazor Components - session from .NET Conf - Focus on Blazor](https://youtu.be/5d-uIxx1cUE)

After you know the basics, pick one of the following testing styles to start with:

- [C# based testing](https://github.com/egil/razor-components-testing-library/wiki/C%23-based-testing)
- [Razor based testing](https://github.com/egil/razor-components-testing-library/wiki/Razor-based-testing)
- [Snapshot testing](https://github.com/egil/razor-components-testing-library/wiki/Snapshot-testing)

If you are unsure, go with _C# based testing_, as it is more stable and resembles the structure of normal unit tests.

If you want a more Razor-native feel when declaring your tests, the _Razor based testing or Snapshot testing_ approach is worth a look. But be warned, the API is likely to change.
