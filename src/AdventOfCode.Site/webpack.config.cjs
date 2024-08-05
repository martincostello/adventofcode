const path = require('path');

module.exports = {
    devtool: 'source-map',
    entry: './scripts/ts/main.ts',
    mode: 'production',
    module: {
        rules: [
            {
                test: /\.ts$/,
                use: 'ts-loader',
                exclude: /node_modules/,
            },
        ],
    },
    output: {
        filename: 'main.js',
        path: path.resolve(__dirname, 'wwwroot', 'static', 'js'),
    },
    resolve: {
        extensions: ['.ts', '.js'],
    },
};
