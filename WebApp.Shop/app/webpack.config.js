module.exports = {
  module: {
    rules: [
      {
        test: /\.css$/i,
        // Probly you already have this rule, add this line
        exclude: /\.lazy\.css$/i,
        use: ["style-loader", "css-loader"],
      },
      // And add this rule
      {
        test: /\.lazy\.css$/i,
        use: [
          { loader: "style-loader", options: { injectType: "lazyStyleTag" } },
          "css-loader",
        ],
      },
    ],
  },
};