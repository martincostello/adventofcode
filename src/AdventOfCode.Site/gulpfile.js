/// <binding ProjectOpened='watch' />
const browserify = require('browserify');
const buffer = require('vinyl-buffer');
const eslint = require('gulp-eslint');
const gulp = require('gulp');
const jest = require('gulp-jest').default;
const prettier = require('gulp-prettier');
const source = require('vinyl-source-stream');
const sourcemaps = require('gulp-sourcemaps');
const tsify = require('tsify');
const uglify = require('gulp-uglify');
const glob = ['scripts/ts/**/*.ts'];

gulp.task('prettier', () => {
    return gulp.src(glob)
        .pipe(prettier())
        .pipe(gulp.dest(file => file.base));
});

gulp.task('lint', () => {
    return gulp
        .src(glob)
        .pipe(eslint({
            formatter: 'visualstudio'
        }))
        .pipe(eslint.format())
        .pipe(eslint.failAfterError());
});

gulp.task('build', () => {
    return browserify({
        basedir: '.',
        debug: true,
        entries: ['scripts/ts/main.ts'],
        cache: {},
        packageCache: {}
    })
        .plugin(tsify)
        .transform('babelify', {
            presets: ['@babel/preset-env'],
            extensions: ['.ts']
        })
        .bundle()
        .pipe(source('main.js'))
        .pipe(buffer())
        .pipe(sourcemaps.init({ loadMaps: true }))
        .pipe(uglify())
        .pipe(sourcemaps.write('./'))
        .pipe(gulp.dest('wwwroot/static/js'));
});

gulp.task('test', () => {
    return gulp
        .src('scripts')
        .pipe(jest());
});

gulp.task('default', gulp.series('prettier', 'lint', 'build'));

gulp.task('watch', () => {
    gulp.watch(glob, gulp.series('lint', 'build', 'test'));
});
