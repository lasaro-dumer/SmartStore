const gulp = require('gulp');
const concat = require('gulp-concat');

const vendorStyles = [
    "node_modules/bootstrap/dist/css/bootstrap.min.css",
    "node_modules/open-iconic/font/css/open-iconic-bootstrap.min.css"
];
const vendorScripts = [
    "node_modules/jquery/dist/jquery.min.js",
    "node_modules/jquery-validation/dist/jquery.validate.min.js",
    "node_modules/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.min.js",
    "node_modules/popper.js/dist/umd/popper.min.js",
    "node_modules/bootstrap/dist/js/bootstrap.min.js",
    "node_modules/chart.js/dist/Chart.min.js"
];
const vendorFonts = [
    "node_modules/open-iconic/font/fonts/open-iconic.woff",
    "node_modules/open-iconic/font/fonts/open-iconic.ttf",
    "node_modules/open-iconic/font/fonts/open-iconic.otf"
];

gulp.task('build-vendor-css', () => {
    return gulp.src(vendorStyles)
        .pipe(gulp.dest('wwwroot/lib/css'));
});

gulp.task('build-vendor-js', () => {
    return gulp.src(vendorScripts)
        .pipe(gulp.dest('wwwroot/lib/js'));
});

gulp.task('build-vendor-fonts', () => {
    return gulp.src(vendorFonts)
        .pipe(gulp.dest('wwwroot/lib/fonts'));
});

gulp.task('build-vendor', gulp.series(['build-vendor-css', 'build-vendor-js', 'build-vendor-fonts']));

gulp.task('email-templates', () => {
    return gulp.src(['EmailTemplates/**/*'])
        .pipe(gulp.dest('wwwroot/EmailTemplates'));
});

gulp.task('default', gulp.series(['build-vendor', 'email-templates']));
