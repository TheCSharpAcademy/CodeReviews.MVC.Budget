const path = require('path');
const webpack = require('webpack');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const { WebpackManifestPlugin } = require('webpack-manifest-plugin');
const CssMinimizerPlugin = require("css-minimizer-webpack-plugin");
const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin;
const CopyWebpackPlugin = require('copy-webpack-plugin');

module.exports = {
    entry: {
        index: './src/js/index-entry.js',
        fiscalPlan: './src/js/fiscalPlan-entry.js',
        category: './src/js/category-entry.js',
    },
    output: {
        filename: '[name].js',
        chunkFilename: "[name].js",
        path: path.resolve(__dirname, '..', 'wwwroot', 'dist'),
        clean: true,
        publicPath: '/dist/'
    },
    mode: 'production',
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
        runtimeChunk: 'single',
        minimize: true,
        minimizer: ['...', new CssMinimizerPlugin({
            parallel: true,
            minimizerOptions: {
                preset: [
                    "default", {
                        discardComments: { removeAll: true }
                    }
                ]
            }
        })],
        splitChunks: {
            cacheGroups: {
                styles: {
                    type: "css/mini-extract",
                    chunks: 'all',
                    enforce: true,
                    reuseExistingChunk: true,
                    name(module, chunks, cacheGroupKey) {
                        const allChunksNames = chunks.map((item) => item.name).join('~');
                        return `${cacheGroupKey}-${allChunksNames}`;
                    },
                },
                defaultVendors: {
                    test: /[\\/]node_modules[\\/]/,
                    chunks: 'initial',
                    priority: -10,
                    reuseExistingChunk: true,
                    name(module, chunks, cacheGroupKey) {
                        let allChunksNames = chunks.map((item) => item.name).join('~');
                        return `vendors${allChunksNames.length > 0 ? `-${allChunksNames}` : ""}`;
                    },
                    filename: (pathData) => {
                        return `${pathData.chunk.name}.js`;
                    },
                },
                default: {
                    chunks: 'initial',
                    minChunks: 2,
                    priority: -20,
                    reuseExistingChunk: true,
                    name(module, chunks, cacheGroupKey) {
                        let allChunksNames = chunks.map((item) => item.name).join('~');
                        return `vendors${allChunksNames.length > 0 ? `-${allChunksNames}` : ""}`;
                    },
                    filename: (pathData) => {
                        return `${pathData.chunk.name}.js`;
                    },
                },
                async: {
                    chunks: 'async',
                    enforce: true,
                    priority: -30,
                    reuseExistingChunk: true,
                    name(module, chunks, cacheGroupKey) {
                        let allChunksNames = chunks.map((item) => item.name).join('~');
                        return `async-${allChunksNames}`;
                    },

                    filename: (pathData) => {
                        return `[name].js`;
                    },
                },
            }
        }
    },
    plugins: [
        new BundleAnalyzerPlugin({ analyzerMode: 'static', openAnalyzer: false }),
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
        new CopyWebpackPlugin({
            patterns: [
                { from: 'src/img', to: 'img' },
            ],
        }),
    ]
};