/*
 * Language: cshtml-razor
 * Requires: xml.js, cs.js, css.js, javascript.js
 * Author: Roman Resh <romanresh@live.com>
*/

var module = module ? module : {};

function getXmlBlocks(hljs, additional_blocks) {
    var xml_comment = hljs.COMMENT(
        '<!--',
        '-->',
        {
            relevance: 10
        }
    );
    var string = {
        className: 'string',
        variants: [
            { begin: /"/, end: /"/ },
            { begin: /'/, end: /'/ },
            { begin: /[^\s"'=<>`]+/ }
        ],
        contains: additional_blocks
    };
    var xml_tag_internal = {
        endsWithParent: true,
        illegal: /</,
        relevance: 0,
        contains: [
            {
                className: 'attr',
                begin: '[A-Za-z0-9\\._:-]+',
                relevance: 0
            },
            {
                begin: /=\s*/,
                relevance: 0,
                contains: [string]
            }
        ]
    };
    return [
        {
            className: 'meta',
            begin: '<!DOCTYPE', end: '>',
            relevance: 10,
            contains: [{ begin: '\\[', end: '\\]' }]
        },
        xml_comment,
        {
            begin: '<\\!\\[CDATA\\[', end: '\\]\\]>',
            relevance: 10
        },
        {
            className: 'meta',
            begin: /<\?xml/, end: /\?>/, relevance: 10
        },
        {
            className: 'tag',
            begin: '<style(?=\\s|>|$)', end: '>',
            keywords: { name: 'style' },
            contains: [xml_tag_internal],
            starts: {
                end: '</style>', returnEnd: true,
                subLanguage: ['css', 'xml']
            }
        },
        {
            className: 'tag',
            begin: '<script(?=\\s|>|$)', end: '>',
            keywords: { name: 'script' },
            contains: [xml_tag_internal],
            starts: {
                end: '\<\/script\>', returnEnd: true,
                subLanguage: ['actionscript', 'javascript', 'handlebars', 'xml']
            }
        },
        {
            className: 'tag',
            begin: '</?', end: '/?>',
            contains: [
                {
                    className: 'name', begin: /[^\/><\s]+/, relevance: 0
                },
                xml_tag_internal
            ]
        }
    ].concat(additional_blocks);
}
function hljsDefineCshtmlRazor(hljs) {
    var SPECIAL_SYMBOL_CLASSNAME = "built_in";
    var CONTENT_REPLACER = {};
    var closed_brace = {
        begin: "}",
        className: SPECIAL_SYMBOL_CLASSNAME,
        endsParent: true
    };
    var braces = {
        begin: "{",
        end: "}",
        contains: [hljs.QUOTE_STRING_MODE, 'self']
    };
    var razor_comment = hljs.COMMENT(
        '@\\*',
        '\\*@',
        {
            relevance: 10
        }
    );
    var razor_inline_expresion = {
        begin: "@[a-zA-Z]+",
        returnBegin: true,
        subLanguage: 'csharp',
        end: "(\\r|\\n|<|\\s|\"|')",
        contains: [
            {
                begin: '@',
                className: SPECIAL_SYMBOL_CLASSNAME
            },
            {
                begin: '".*(?!$)"',
                skip: true
            },
            {
                begin: '"',
                endsParent: true
            }
        ],
        returnEnd: true
    };
    var razor_text_block = {
        begin: "[@]{0,1}<text>",
        returnBegin: true,
        end: "</text>",
        returnEnd: true,
        subLanguage: "cshtml-razor",
        contains: [
            {
                begin: "[@]{0,1}<text>",
                className: SPECIAL_SYMBOL_CLASSNAME
            },
            {
                begin: "</text>",
                className: SPECIAL_SYMBOL_CLASSNAME,
                endsParent: true
            }
        ]
    };
    var razor_escape_at = {
        variants: [
            { begin: "@@" },
            { begin: "[a-zA-Z]+@" }
        ],
        skip: true
    };

    var razor_parentheses_block = {
        begin: "@\\(",
        end: "\\)",
        returnBegin: true,
        returnEnd: true,
        subLanguage: "csharp",
        contains: [
            {
                begin: "@\\(",
                className: SPECIAL_SYMBOL_CLASSNAME
            },
            {
                begin: "\\(",
                end: "\\)",
                subLanguage: 'csharp',
                contains: [hljs.QUOTE_STRING_MODE, 'self', razor_text_block]
            },
            razor_text_block,
            {
                begin: "\\)",
                className: SPECIAL_SYMBOL_CLASSNAME,
                endsParent: true
            }
        ]
    };
    var xml_blocks = getXmlBlocks(hljs, [razor_inline_expresion, razor_parentheses_block]);
    var razor_directives = {
        begin: "^\\s*@(page|model|using|inherits|inject)[^\\r\\n{\\(]*$",
        end: "$",
        returnBegin: true,
        returnEnd: true,
        contains: [
            {
                begin: "^\\s*@(page|model|using|inherits|inject)",
                className: SPECIAL_SYMBOL_CLASSNAME
            },
            {
                variants: [
                    { begin: "\\r|\\n", endsParent: true},
                    { begin: "\\s[^\\r\\n]+", end: "$" },
                    { begin: "$" }
                ],
                className: "type",
                endsParent: true
            }
        ]
    };
    var razor_block = {
        begin: "@\\{",
        returnBegin: true,
        returnEnd: true,
        end: "\\}",
        subLanguage: 'csharp',
        contains: [
            {
                begin: "@\\{",
                className: SPECIAL_SYMBOL_CLASSNAME
            },
            CONTENT_REPLACER,
            closed_brace
        ]
    };
    var razor_helper_block = {
        begin: "^\\s*@helper[\\s]*[^{]+[\\s]*{",
        returnBegin: true,
        returnEnd: true,
        end: "}",
        subLanguage: "cshtml-razor",
        contains: [
            { begin: "@helper", className: SPECIAL_SYMBOL_CLASSNAME },
            { begin: "{", className: SPECIAL_SYMBOL_CLASSNAME },
            closed_brace
        ]
    };
    var razor_code_block_variants = [
        { begin: "@for[\\s]*\\([^{]+[\\s]*{", end: "}" },
        { begin: "@if[\\s]*\\([^{]+[\\s]*{", end: "}" },
        { begin: "@switch[\\s]*\\([^{]+[\\s]*{", end: "}" },
        { begin: "@while[\\s]*\\([^{]+[\\s]*{", end: "}" },
        { begin: "@using[\\s]*\\([^{]+[\\s]*{", end: "}" },
        { begin: "@lock[\\s]*\\([^{]+[\\s]*{", end: "}" },
        { begin: "@foreach[\\s]*\\([^{]+[\\s]*{", end: "}" }
    ];
    var razor_code_block = {
        variants: razor_code_block_variants,
        returnBegin: true,
        returnEnd: true,
        end: "}",
        subLanguage: 'csharp',
        contains: [
            {
                variants: razor_code_block_variants.map(function (v) { return { begin: v.begin }; }),
                returnBegin: true,
                contains: [
                    { begin: "@", className: SPECIAL_SYMBOL_CLASSNAME },
                    {
                        variants: razor_code_block_variants.map(function (v) { return { begin: v.begin.substr(1, v.begin.length - 2) }; }),
                        subLanguage: "csharp"
                    },
                    { begin: "{", className: SPECIAL_SYMBOL_CLASSNAME }
                ]
            },
            CONTENT_REPLACER,
            {
                variants: [
                    { begin: "}[\\s]*else\\sif[\\s]*\\([^{]+[\\s]*{" },
                    { begin: "}[\\s]*else[\\s]*{" }
                ],
                returnBegin: true,
                contains: [
                    { begin: "}", className: SPECIAL_SYMBOL_CLASSNAME },
                    {
                        variants: [
                            { begin: "[\\s]*else\\sif[\\s]*\\([^{]+[\\s]*{" },
                            { begin: "[\\s]*else[\\s]*" }
                        ],
                        subLanguage: "csharp"
                    },
                    {
                        begin: "{",
                        className: SPECIAL_SYMBOL_CLASSNAME
                    }
                ]
            },
            braces,
            closed_brace
        ]
    };
    var section_begin = "@section[\\s]+[a-zA-Z0-9]+[\\s]*{";
    var razor_try_block = {
        begin: "@try[\\s]*{",
        end: "}",
        returnBegin: true,
        returnEnd: true,
        subLanguage: "csharp",
        contains: [
            { begin: "@", className: SPECIAL_SYMBOL_CLASSNAME },
            { begin: "try[\\s]*{", subLanguage: "csharp" },
            {
                variants: [
                    { begin: "}[\\s]*catch[\\s]*\\([^\\)]+\\)[\\s]*{" },
                    { begin: "}[\\s]*finally[\\s]*{" }
                ],
                returnBegin: true,
                contains: [
                    { begin: "}", className: SPECIAL_SYMBOL_CLASSNAME },
                    {
                        variants: [
                            { begin: "[\\s]*catch[\\s]*\\([^\\)]+\\)[\\s]*", },
                            { begin: "[\\s]*finally[\\s]*", },
                        ],
                        subLanguage: "csharp"
                    },
                    { begin: "{", className: SPECIAL_SYMBOL_CLASSNAME }
                ]
            },
            CONTENT_REPLACER,
            braces,
            closed_brace
        ]
    };
    var razor_section_block = {
        begin: section_begin,
        returnBegin: true,
        returnEnd: true,
        end: "}",
        subLanguage: 'cshtml-razor',
        contains: [
            {
                begin: section_begin,
                className: SPECIAL_SYMBOL_CLASSNAME
            },
            braces,
            closed_brace
        ]
    };
    var rasor_await = {
        begin: "@await ",
        returnBegin: true,
        subLanguage: 'csharp',
        end: "(\\r|\\n|<|\\s)",
        contains: [
            {
                begin: "@await ",
                className: SPECIAL_SYMBOL_CLASSNAME
            },
            {
                begin: "[<\\r\\n]",
                endsParent: true
            }
        ]
    };

    var result = {
        aliases: ['cshtml', 'razor', 'razor-cshtml'],
        contains: [
            razor_directives,
            razor_helper_block,
            razor_block,
            razor_code_block,
            razor_section_block,
            rasor_await,
            razor_try_block,
            razor_escape_at,
            razor_text_block,
            razor_comment,
            razor_parentheses_block,
            {
                className: 'meta',
                begin: '<!DOCTYPE', end: '>',
                relevance: 10,
                contains: [{ begin: '\\[', end: '\\]' }]
            },
            {
                begin: '<\\!\\[CDATA\\[', end: '\\]\\]>',
                relevance: 10
            }
        ]
    };
    result.contains = result.contains.concat(xml_blocks);

    [razor_block, razor_code_block, razor_try_block]
        .forEach(function (mode) {
            var razorModes = result.contains.filter(function (c) { return c !== mode; });
            var replacerIndex = mode.contains.indexOf(CONTENT_REPLACER);
            mode.contains.splice.apply(mode.contains, [replacerIndex, 1].concat(razorModes));
        });

    return result;
}

module.exports = function (hljs) {
    hljs.registerLanguage('cshtml-razor', hljsDefineCshtmlRazor);
};

module.exports.definer = hljsDefineCshtmlRazor;
