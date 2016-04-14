﻿/// <binding AfterBuild='less' />
"use strict";

var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify"),
    sourcemaps = require("gulp-sourcemaps"),
    rigger = require("gulp-rigger"),
    prefixer = require("gulp-autoprefixer"),
    less = require("gulp-less"),
    watch = require("gulp-watch");

var paths = {
    webroot: "./wwwroot/",
    assets: "./Assets/"
};

paths.js = paths.webroot + "js/**/*.js";
paths.minJs = paths.webroot + "js/**/*.min.js";
paths.css = paths.webroot + "css/**/*.css";
paths.minCss = paths.webroot + "css/**/*.min.css";
paths.concatJsDest = paths.webroot + "js/site.min.js";
paths.concatCssDest = paths.webroot + "css/site.min.css";
paths.less = paths.assets + "Stylesheets/Main.less";

gulp.task("build:less", function() {
    return gulp.src(paths.less)
        .pipe(rigger())
        .pipe(sourcemaps.init())
        .pipe(less())
        .pipe(prefixer())
        .pipe(cssmin())
        .pipe(sourcemaps.write())
        .pipe(gulp.dest(paths.webroot + "/css"));
});

gulp.task("clean:js", function (cb) {
    rimraf(paths.concatJsDest, cb);
});

gulp.task("clean:css", function (cb) {
    rimraf(paths.concatCssDest, cb);
});

gulp.task("clean", ["clean:js", "clean:css"]);

gulp.task("min:js", function () {
    return gulp.src([paths.js, "!" + paths.minJs], { base: "." })
        .pipe(concat(paths.concatJsDest))
        .pipe(uglify())
        .pipe(gulp.dest("."));
});

gulp.task("min:css", function () {
    return gulp.src([paths.css, "!" + paths.minCss])
        .pipe(concat(paths.concatCssDest))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});

gulp.task("watch:less", function() {
    watch([paths.assets + "/Stylesheets/**/*.less"], function() {
        gulp.start('build:less');
    });
});

gulp.task("min", ["min:js", "min:css"]);

gulp.task("default", ["less", "min"]);