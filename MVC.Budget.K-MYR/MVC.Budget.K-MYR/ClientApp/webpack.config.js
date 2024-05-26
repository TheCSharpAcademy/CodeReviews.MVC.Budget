const path = require('path');
const webpack = require('webpack');
const TerserPlugin = require('terser-webpack-plugin');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const { WebpackManifestPlugin }  = require('webpack-manifest-plugin');

module.exports = {
    entry: {
        index: './src/js/index-entry.js',
        index1: './src/js/index1-entry.js'

    },
    output: {
        filename: '[name].[contenthash].js',
        path: path.resolve(__dirname, '..', 'wwwroot', 'dist'),
        clean: true,
        publicPath: '/dist/'
    },
    devtool: 'inline-source-map',
    mode: 'development',  
    module: {
        rules: [
            {
                test: /\.s?css$/,
                use: [{ loader: MiniCssExtractPlugin.loader }, 'css-loader', 'sass-loader']
            },
            {
                test: /\.(png|svg|jpg|jpeg|gif|webp)$/i,
                type: 'asset'
            },
            {
                test: /\.(eot|woff(2)?|ttf|otf|svg)$/i,
                type: 'asset'
            }
        ]
    },
    optimization: {  
        minimize: true,
        minimizer: [new TerserPlugin()],
    },
    plugins: [
        new webpack.ProvidePlugin({
            $: 'jquery',
            jQuery: 'jquery',
            'window.jQuery': 'jquery'
        }),
        new MiniCssExtractPlugin({
            filename: "[name].css"
        }),
        new WebpackManifestPlugin(
            {
                fileName: 'prod.manifest.json',
                generate: (seed, files, entries) => {
                    return entries;
                }
            }),
    ]
};