grammar LinkLookupInHtml;

html: (.*? '<a' .*? 'href' SPACE* '=' SPACE* QUOTE links+=LINK QUOTE .*? '>' .*? '</a>' .*?)*;

LINK: [a-zA-Z0-9:/?\\.%#!&-]+;
SPACE: ' ';
QUOTE: '"';
WHITESPACE: [ \t\r\n\u00A0] -> skip;