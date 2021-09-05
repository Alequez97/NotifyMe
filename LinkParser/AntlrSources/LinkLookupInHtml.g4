grammar LinkLookupInHtml;

html: (.*? '<a' .*? 'href' SPACE* '=' SPACE* '\"' links+=LINK '\"' .*? '>' .*? '</a>' .*?)*;

LINK: [a-zA-Z0-9:/?\\.&]+;
SPACE: ' ';
WHITESPACE: [ \t\r\n\u00A0] -> skip;