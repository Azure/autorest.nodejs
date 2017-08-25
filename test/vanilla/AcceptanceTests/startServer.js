"use strict";
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
exports.__esModule = true;
var fs = require("fs");
var child_process = require("child_process");
var child;
before(function (done) {
    var isWin = /^win/.test(process.platform);
    var nodeCmd = 'node.exe';
    if (!isWin) {
        nodeCmd = 'node';
    }
    var started = false;
    var out = fs.openSync('./server.log', 'w');
    fs.writeSync(out, 'Test run started at ' + new Date().toISOString() + '\n');
    child = child_process.spawn(nodeCmd, [__dirname + '/../../../node_modules/@microsoft.azure/autorest.testserver']);
    child.stdout.on('data', function (data) {
        fs.writeSync(out, data.toString('UTF-8'));
        if (data.toString().indexOf('started') > 0) {
            started = true;
            done();
        }
    });
    child.on('close', function () {
        if (!started) {
            done();
        }
    });
});
after(function (done) {
    child.kill();
    done();
});
