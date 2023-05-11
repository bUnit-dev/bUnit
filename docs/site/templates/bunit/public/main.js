import { razor } from './cshtml-razor.min.js'

export default {
  iconLinks: [
    {
      icon: 'github',
      href: 'https://github.com/bUnit-dev/bUnit',
      title: 'GitHub'
    },
    {
      icon: 'patch-question-fill',
      href: 'https://github.com/bUnit-dev/bUnit/discussions/categories/q-a',
      title: 'Q/A forum on GitHub'
    },
    {
      icon: 'stack-overflow',
      href: 'https://stackoverflow.com/tags/bunit',
      title: 'Q/A forum on StackOverflow'
    },
    {
      icon: 'twitter',
      href: 'https://twitter.com/bUnitBot',
      title: 'Twitter'
    }
  ],
  configureHljs: function (hljs) {
    hljs.registerLanguage("cshtml-razor", razor);
  },
}
