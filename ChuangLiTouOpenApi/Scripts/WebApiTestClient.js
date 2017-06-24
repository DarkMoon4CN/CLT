var testClientModel;
var emptyTestClientModel =
{
    HttpMethod: '',
    UriPathTemplate: '',
    Samples: {},
    UriParameters: [],
    BaseAddress: '/'
};

(function () {
    function BuildUriPath(template, uriParameters) {
        var path = template;
        for (var i in uriParameters) {
            var parameter = uriParameters[i];
            if (parameter.enabled()) {
                var parameterValue = parameter.value();
                if (parameterValue != "") {
                    var variableName = '{' + parameter.name + '}';
                    path = path.replace(variableName, parameterValue);
                }
            }
            else {
                path = RemoveUriParameter(path, parameter.name)
            }
        }

        // cleanup path
        path = path.replace("/?", "?");
        path = path.replace("?&", "?");

        // remove trailing '?'
        if (path.charAt(path.length - 1) == '?') {
            path = path.substr(0, path.length - 1);
        }
        // remove trailing '&'
        if (path.charAt(path.length - 1) == '&') {
            path = path.substr(0, path.length - 1);
        }

        return path;
    }

    function RemoveUriParameter(template, parameterToRemove) {
        var path = template;
        var urlParameter = '{' + parameterToRemove + '}';
        var queryParameter = parameterToRemove + '=' + urlParameter;
        path = path.replace(queryParameter, "");
        path = path.replace(urlParameter, "");
        return path;
    }

    function TestClientViewModel(data) {
        var self = this;
        self.HttpMethod = ko.observable(data.HttpMethod);
        self.UriPathTemplate = data.UriPathTemplate;
        self.UriPath = ko.observable(self.UriPathTemplate);

        self.UriParameters = new Array();
        for (var i in data.UriParameters) {
            var uriParameter = data.UriParameters[i];
            var uriParameterValue = ko.observable(uriParameter.value);
            var parameterEnabled = ko.observable(true);
            uriParameterValue.subscribe(function () {
                self.UriPath(BuildUriPath(self.UriPathTemplate, self.UriParameters));
            });
            parameterEnabled.subscribe(function () {
                self.UriPath(BuildUriPath(self.UriPathTemplate, self.UriParameters));
            });
            self.UriParameters.push({ name: uriParameter.name, value: uriParameterValue, enabled: parameterEnabled });
        }

        self.RequestHeaders = ko.observableArray();

        self.RequestMediaType = ko.observable();

        var sampleTypes = new Array();
        for (var index in data.Samples) {
            sampleTypes.push(index);
        };
        self.SampleTypes = sampleTypes;

        self.ShouldShowBody = ko.observable(sampleTypes.length > 0);

        self.RequestBody = ko.observable();

        self.RequestMediaType.subscribe(function () {
            //  self.RequestBody(decodeSample(data.Samples[self.RequestMediaType()]) || "");
            var headers = self.RequestHeaders;
            var mediaType = self.RequestMediaType();
            if (mediaType && mediaType != "") {
                addOrReplaceHeader(headers, "content-type", mediaType);
            }
        });

        self.RequestBody.subscribe(function () {
            var headers = self.RequestHeaders;
            var contentLengh = self.RequestBody().length;
            addOrReplaceHeader(headers, "content-length", contentLengh);
        });

        self.addHeader = function () {
            self.RequestHeaders.splice(0, 0, { name: "", value: "" });
        };

        self.removeHeader = function (header) {
            self.RequestHeaders.remove(header);
        };

        self.response = ko.observable();

        self.sendRequest = function () {
            var uriPath = self.UriPath();
            var http = "http://";
            var https = "https://";
            // Just take the entire uriPath if it's an absolute URI.
            var uri = (uriPath.slice(0, http.length) == http || uriPath.slice(0, https.length) == https) ?
                uriPath :
                data.BaseAddress + uriPath;

            var httpMethod = self.HttpMethod().toLowerCase();
            var headers = self.RequestHeaders();
            var requestBody = self.ShouldShowBody() ? self.RequestBody() : null;



            if (httpMethod == "post") {
                $.ajax({
                    type: 'POST',
                    url: uri,
                    data: JSON.stringify(eval(requestBody)),
                    contentType: 'application/json',
                    dataType: 'json',
                    success: function (res) {
                        var httpResponse = getHttpResponse(res);
                        self.response(httpResponse);
                        $("#testClientResponseDialog").dialog("open");
                    }
                });


            } else {

                SendRequest(httpMethod, uri, headers, requestBody, function (httpRequest) {
                    var httpResponse = getHttpResponse(httpRequest);
                    self.response(httpResponse);
                    $("#testClientResponseDialog").dialog("open");
                });
            }

        };

        $("#testClientDialog").dialog({
            autoOpen: false,
            height: "auto",
            width: "700",
            modal: true,
            open: function () {
                jQuery('.ui-widget-overlay').bind('click', function () {
                    jQuery('#testClientDialog').dialog('close');
                })
            },
            buttons: {
                "Send": function () {
                    self.sendRequest();
                }
            }
        });

        $("#testClientResponseDialog").dialog({
            autoOpen: false,
            height: "auto",
            width: "550",
            modal: true,
            open: function () {
                jQuery('.ui-widget-overlay').bind('click', function () {
                    jQuery('#testClientResponseDialog').dialog('close');
                })
            }
        });

        $("#testClientButton").click(function () {
            $("#testClientDialog").dialog("open");
        });
    }

    // Initiate the Knockout bindings
    var initialModel = testClientModel || emptyTestClientModel;
    ko.applyBindings(new TestClientViewModel(initialModel));
})();

function decodeSample(sampleString) {
    return unescape(sampleString).replace(/\+/gi, " ").replace(/\r\n/gi, "\n");
}

function addOrReplaceHeader(headers, headerName, headerValue) {
    var headerList = headers();
    for (var i in headerList) {
        if (headerList[i].name.toLowerCase() == headerName) {
            headers.replace(headerList[i], { name: headerList[i].name, value: headerValue });
            return;
        }
    }
    headers.push({ name: headerName, value: headerValue });
}

function SendRequest(httpMethod, url, requestHeaders, requestBody, handleResponse) {
    if (httpMethod.length == 0) {
        alert("HTTP Method should not be empty");
        return false;
    }

    if (url.length == 0) {
        alert("Url should not be empty");
        return false;
    }

    var httpRequest = new XMLHttpRequest();
    try {
        httpRequest.open(httpMethod, encodeURI(url), false);
    }
    catch (e) {
        alert("Cannot send request. Check the security setting of your browser if you are sending request to a different domain.");
        return false;
    }

    try {
        for (var i in requestHeaders) {
            var header = requestHeaders[i];
            httpRequest.setRequestHeader(header.name, header.value);
        }
    } catch (e) {
        alert("Invalid header.");
        return false;
    }

    httpRequest.onreadystatechange = function () {
        switch (this.readyState) {
            case 4:
                handleResponse(httpRequest);
                break;
            default:
                break;
        }
    }

    httpRequest.ontimeout = function () {
        alert("Request timed out.");
    }

    try {
        httpRequest.send(requestBody);
    } catch (e) {
        alert(e);
        return false;
    }

    return true;
}

function getHttpResponse(httpRequest) {

    var statusCode = httpRequest.code;
    var statusText = httpRequest.message;
    var rawResponse = formatJson(JSON.stringify(httpRequest.body));

    if (statusCode === 1223) {
        statusCode = 204;
        statusText = "No Content";
    }
     

    return { status: statusCode, statusText: statusText, content: rawResponse };
}


var formatJson = function (json, options) {
    var reg = null,
		formatted = '',
		pad = 0,
		PADDING = '    '; // one can also use '\t' or a different number of spaces

    // optional settings
    options = options || {};
    // remove newline where '{' or '[' follows ':'
    options.newlineAfterColonIfBeforeBraceOrBracket = (options.newlineAfterColonIfBeforeBraceOrBracket === true) ? true : false;
    // use a space after a colon
    options.spaceAfterColon = (options.spaceAfterColon === false) ? false : true;

    // begin formatting...
    if (typeof json !== 'string') {
        // make sure we start with the JSON as a string
        json = JSON.stringify(json);
    } else {
        // is already a string, so parse and re-stringify in order to remove extra whitespace
        json = JSON.parse(json);
        json = JSON.stringify(json);
    }

    // add newline before and after curly braces
    reg = /([\{\}])/g;
    json = json.replace(reg, '\r\n$1\r\n');

    // add newline before and after square brackets
    reg = /([\[\]])/g;
    json = json.replace(reg, '\r\n$1\r\n');

    // add newline after comma
    reg = /(\,)/g;
    json = json.replace(reg, '$1\r\n');

    // remove multiple newlines
    reg = /(\r\n\r\n)/g;
    json = json.replace(reg, '\r\n');

    // remove newlines before commas
    reg = /\r\n\,/g;
    json = json.replace(reg, ',');

    // optional formatting...
    if (!options.newlineAfterColonIfBeforeBraceOrBracket) {
        reg = /\:\r\n\{/g;
        json = json.replace(reg, ':{');
        reg = /\:\r\n\[/g;
        json = json.replace(reg, ':[');
    }
    if (options.spaceAfterColon) {
        reg = /\:/g;
        json = json.replace(reg, ': ');
    }

    $.each(json.split('\r\n'), function (index, node) {
        var i = 0,
			indent = 0,
			padding = '';

        if (node.match(/\{$/) || node.match(/\[$/)) {
            indent = 1;
        } else if (node.match(/\}/) || node.match(/\]/)) {
            if (pad !== 0) {
                pad -= 1;
            }
        } else {
            indent = 0;
        }

        for (i = 0; i < pad; i++) {
            padding += PADDING;
        }

        formatted += padding + node + '\r\n';
        pad += indent;
    });

    return formatted;
};