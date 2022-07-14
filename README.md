# MarkdownFromHtml

Converts HTML to [Markdown](http://daringfireball.net/projects/markdown/syntax).


## Support

This project will currently convert the following HTML tags:-

| category             | tag                           | category  | tag               |
|----------------------|-------------------------------|-----------|-------------------|
| thematic break       | `<hr>`                        | bold      | `<strong>`, `<b>` |
| heading              | `<h1>`, `<h2>`, `<h3>`,       | code span | `<code>`          |
|                      | `<h4>`, `<h5>`, `<h6>`        | hyperlink | `<a>`             |
| indented code blocks | `<pre><code class="lang-**">` | image     | `<img>`           |
| fenced code blocks   | `<pre><code>`                 | italic    | `<em>`, `<i>`     |
| paragraph            | `<p>`                         | linebreak | `<br>`            |
| block quote          | `<blockquote>`                |
| list                 | `<ul>`, `<ol>`                |


And extensions add

| category        | tag                                | category      | tag      |
|-----------------|------------------------------------|---------------|----------|
| citation        | `<cite>`                           | underline     | `<ins>`  |
| figure          | `<figure>`                         | strikethrough | `<del>`  |
| footer          | `<footer>`                         | subscript     | `<sub>`  |
| pipe table      | `<table>`                          | superscript   | `<sup>`  |
| grid table      | `<table>`                          | marked text   | `<mark>` |

## Nuget

https://www.nuget.org/packages/MarkdownFromHtml


## Usage

### Strings

```csharp
var html = "Something to <strong>convert</strong>";
var converter = new Converter();
var markdown = converter.Convert(html);
```


### Files

```csharp
var path = "file.html";
var converter = new Converter();
var markdown = converter.ConvertFile(path);
```


## Customise

`ReplaceManager` used to build the converter accepts other parsers.
It can enable extension syntax; table, del(strikethrough), etc.

```cs
// using MarkdownFromHtml.Parsers.MarkdigExtensions;

var manager = new ReplaceManager();
manager.Register(new GridTableParser());
manager.Register(new PipeTableParser());

ver converter = new Converter(manager);
```


## Try it

If you can run WebAssembly, please see [demoapps](https://whistyun.github.io/MarkdownFromHtml/demo/index.html).

![screenshot](docs/demo_shot.png)


## License

This project is licensed under [Apache License 2.0](http://www.apache.org/licenses/LICENSE-2.0).