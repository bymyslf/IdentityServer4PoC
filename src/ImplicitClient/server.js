// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

var port = 5003;
var url = "http://localhost:" + port;

var express = require('express');
var app = express();

var static = express.static('public');
app.use(function (req, res, next) {
    res.set('Content-Security-Policy', "default-src 'self'");
    next();
  });
app.use(static);

var oidc = require('./oidc.js');
oidc(url, app);

console.log("listening on " + url);

app.listen(port);
