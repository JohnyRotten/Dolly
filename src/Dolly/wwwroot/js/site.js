﻿$(function() {
    function aload(t) { "use strict"; t = t || window.document.querySelectorAll("[data-aload]"), void 0 === t.length && (t = [t]); var a, e = 0, r = t.length; for (e; r > e; e += 1) a = t[e], a["LINK" !== a.tagName ? "src" : "href"] = a.getAttribute("data-aload"), a.removeAttribute("data-aload"); return t }
    aload();
});