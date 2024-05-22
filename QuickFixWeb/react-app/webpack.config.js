const path = require('path');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const webpack = require('webpack');

module.exports = {
  entry: './src/index.js',
  output: {
    path: path.resolve(__dirname, '../wwwroot/dist'),
    filename: 'bundle.js'
  },
  module: {
    rules: [
      {
        test: /\.jsx?$/,
        exclude: /node_modules/,
        use: {
          loader: 'babel-loader',
          options: {
            presets: ['@babel/preset-react']
          }
        }
      },
      {
        test: /\.css$/,
        use: [MiniCssExtractPlugin.loader, 'css-loader'],
      },
      {
        test: /\.(png|jpg|gif|svg)$/,  // Add this rule to handle image files including SVG
        use: 'file-loader',
      }
    ]
  },
  plugins: [
    new MiniCssExtractPlugin({
      filename: 'styles.css',
    })
  ],
  //mode: process.env.NODE_ENV === 'production' ? 'production' : 'development',
  resolve: {
    extensions: ['.js', '.jsx']
  }
};
