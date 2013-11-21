var name = [];
var value = [];

$.urlParam = function (name) {
    var results = new RegExp('[\\?&]' + name + '=([^&#]*)').exec(window.location.href);
    if (results == null) {
        return null;
    }
    else {
        return results[1] || 0;
    }
}

function overallChart() {
    var overallAnalysis = [['Abuse', value[1]],
                            ['Not Abuse', 100 - value[1]]];
    $.jqplot('overallAnalysis', [overallAnalysis],
    {
        title: 'Overall Analysis',
        seriesDefaults: {
            // Make this a pie chart.
            renderer: jQuery.jqplot.PieRenderer,
            rendererOptions: {
                // Put data labels on the pie slices.
                // By default, labels show the percentage of the slice.
                showDataLabels: true
            }
        },
        legend: { show: true, location: 'e' }
    });
}

function faceChart() {
    //Face Analysis Pie Chart
    var faceAnalysis = [['Smile', value[3]],
                        ['Angry', value[4]],
                        ['Sad', value[5]],
                        ['Neutral', value[6]],
                        ['No Detect', value[2] - value[3] - value[4] - value[5] - value[6]]];
    $.jqplot('faceAnalysis', [faceAnalysis],
    {
        title: 'Face Analysis',
        seriesDefaults: {
            // Make this a pie chart.
            renderer: jQuery.jqplot.PieRenderer,
            rendererOptions: {
                // Put data labels on the pie slices.
                // By default, labels show the percentage of the slice.
                showDataLabels: true
            }
        },
        legend: { show: true, location: 'e' }
    });
}

function handChart() {
    //Hand Motion Analysis Pie Chart
    var handMotion = [['Left Fist', value[7]],
                        ['Right Fist', value[8]],
                        ['Left Palm', value[9]],
                        ['Right Palm', value[10]],
                        ['No Detect', value[2] - value[7] - value[8] - value[9] - value[10]]];
    $.jqplot('handMotion', [handMotion],
    {
        title: 'Hand Motion Analysis',
        seriesDefaults: {
            // Make this a pie chart.
            renderer: jQuery.jqplot.PieRenderer,
            rendererOptions: {
                // Put data labels on the pie slices.
                // By default, labels show the percentage of the slice.
                showDataLabels: true
            }
        },
        legend: { show: true, location: 'e' }
    });
}

function soundChart() {
    var soundPer = 0;
    if (value[11] == true) {
        soundPer = 100;
    }
    var sound = [['True', soundPer],
                    ['False', 100 - soundPer]];
    $.jqplot('soundAnalysis', [sound],
    {
        title: 'Sound Analysis',
        seriesDefaults: {
            // Make this a pie chart.
            renderer: jQuery.jqplot.PieRenderer,
            rendererOptions: {
                // Put data labels on the pie slices.
                // By default, labels show the percentage of the slice.
                showDataLabels: true
            }
        },
        legend: { show: true, location: 'e' }
    });
}

function feverChart() {
    var fever = [['Fever', value[13]],
                    ['No Fever', 100 - value[13]]];
    $.jqplot('feverAnalysis', [fever],
    {
        title: 'Fever Analysis',
        seriesDefaults: {
            // Make this a pie chart.
            renderer: jQuery.jqplot.PieRenderer,
            rendererOptions: {
                // Put data labels on the pie slices.
                // By default, labels show the percentage of the slice.
                showDataLabels: true
            }
        },
        legend: { show: true, location: 'e' }
    });
}

function twitterChart() {
    var twitter = [['Tweets with word "Abuse"', 32],
                    ['Tweets with word "Child"', 35.99],
                    ['Tweets with external links', 11.99],
                    ['Tweets with word "Test"', 8.03],
                    ['Other Tweets', 11.99]];
    $.jqplot('twitterAna', [twitter],
    {
        title: 'Twitter Timeline Analysis',
        seriesDefaults: {
            // Make this a pie chart.
            renderer: jQuery.jqplot.PieRenderer,
            rendererOptions: {
                // Put data labels on the pie slices.
                // By default, labels show the percentage of the slice.
                showDataLabels: true
            }
        },
        legend: { show: true, location: 'e' }
    });
}

$(document).ready(function () {
    var id = $.urlParam('ID');
    var data_file = "http://localhost:27020/api/json/analysis/" + id;
    var http_request = new XMLHttpRequest();
    try {
        // Opera 8.0+, Firefox, Chrome, Safari
        http_request = new XMLHttpRequest();
    } catch (e) {
        // Internet Explorer Browsers
        try {
            http_request = new ActiveXObject("Msxml2.XMLHTTP");
        } catch (e) {
            try {
                http_request = new ActiveXObject("Microsoft.XMLHTTP");
            } catch (e) {
                // Something went wrong
                alert("Your browser broke!");
                return false;
            }
        }
    }

    http_request.onreadystatechange = function () {
        if (http_request.readyState == 4) {
            // Javascript function JSON.parse to parse JSON data
            var jsonObj = JSON.parse(http_request.responseText);
            var a = 0;
            for (var att in jsonObj.Analysis) {
                name[a] = att;
                value[a] = jsonObj.Analysis[att];
                a++;
            }
            overallChart();
        }
    }
    http_request.open("GET", data_file, true);
    http_request.send();

    var i = 1;
    $("#next").css("opacity", "0.4");
    $("#prev").css("opacity", "0.4");
    $("#faceAnalysis").hide();
    $("#handMotion").hide();
    $("#soundAnalysis").hide();
    $("#feverAnalysis").hide();
    $("#twitterAna").hide();

    $("#next").mouseover(function () {
        $(this).css("opacity", "1");
    }).mouseout(function () {
        $(this).css("opacity", "0.4");
    }).click(function () {
        if (i == 0) {
            i++;
            $("#overallAnalysis").show();
            $("#faceAnalysis").hide();
            $("#faceAnalysis").empty();
            $("#handMotion").hide();
            $("#handMotion").empty();
            $("#soundAnalysis").hide();
            $("#soundAnalysis").empty();
            $("#feverAnalysis").hide();
            $("#feverAnalysis").empty();
            $("#twitterAna").hide();
            $("#twitterAna").empty();
            overallChart();
        }
        else if (i == 1) {
            i++;
            $("#overallAnalysis").hide();
            $("#overallAnalysis").empty();
            $("#faceAnalysis").show();
            $("#handMotion").hide();
            $("#handMotion").empty();
            $("#soundAnalysis").hide();
            $("#soundAnalysis").empty();
            $("#feverAnalysis").hide();
            $("#feverAnalysis").empty();
            $("#twitterAna").hide();
            $("#twitterAna").empty();
            faceChart();
        }
        else if (i == 2) {
            i++;
            $("#overallAnalysis").hide();
            $("#overallAnalysis").empty();
            $("#faceAnalysis").hide();
            $("#faceAnalysis").empty();
            $("#handMotion").show();
            $("#soundAnalysis").hide();
            $("#soundAnalysis").empty();
            $("#feverAnalysis").hide();
            $("#feverAnalysis").empty();
            $("#twitterAna").hide();
            $("#twitterAna").empty();
            handChart();
        }
        else if (i == 3) {
            i++;
            $("#overallAnalysis").hide();
            $("#overallAnalysis").empty();
            $("#faceAnalysis").hide();
            $("#faceAnalysis").empty();
            $("#handMotion").hide();
            $("#handMotion").empty();
            $("#soundAnalysis").show();
            $("#feverAnalysis").hide();
            $("#feverAnalysis").empty();
            $("#twitterAna").hide();
            $("#twitterAna").empty();
            soundChart();
        }
        else if (i == 4) {
            i++;
            $("#overallAnalysis").hide();
            $("#overallAnalysis").empty();
            $("#faceAnalysis").hide();
            $("#faceAnalysis").empty();
            $("#handMotion").hide();
            $("#handMotion").empty();
            $("#soundAnalysis").hide();
            $("#soundAnalysis").empty();
            $("#feverAnalysis").show();
            $("#twitterAna").hide();
            $("#twitterAna").empty();
            feverChart();
        }
        else if (i == 5) {
            i++;
            $("#overallAnalysis").hide();
            $("#overallAnalysis").empty();
            $("#faceAnalysis").hide();
            $("#faceAnalysis").empty();
            $("#handMotion").hide();
            $("#handMotion").empty();
            $("#soundAnalysis").hide();
            $("#soundAnalysis").empty();
            $("#feverAnalysis").hide();
            $("#feverAnalysis").empty();
            $("#twitterAna").show();
            twitterChart();
        }
        else {
            i = 1;
            $("#overallAnalysis").show();
            $("#faceAnalysis").hide();
            $("#faceAnalysis").empty();
            $("#handMotion").hide();
            $("#handMotion").empty();
            $("#soundAnalysis").hide();
            $("#soundAnalysis").empty();
            $("#feverAnalysis").hide();
            $("#feverAnalysis").empty();
            $("#twitterAna").hide();
            $("#twitterAna").empty();
            overallChart();
        }
    });

    $("#prev").mouseover(function () {
        $(this).css("opacity", "1");
    }).mouseout(function () {
        $(this).css("opacity", "0.4");
    }).click(function () {
        if (i == 5) {
            i = i - 1;
            $("#overallAnalysis").hide();
            $("#overallAnalysis").empty();
            $("#faceAnalysis").hide();
            $("#faceAnalysis").empty();
            $("#handMotion").hide();
            $("#handMotion").empty();
            $("#soundAnalysis").hide();
            $("#soundAnalysis").empty();
            $("#feverAnalysis").hide();
            $("#feverAnalysis").empty();
            $("#twitterAna").show();
            twitterChart();
        }
        else if (i == 4) {
            i = i - 1;
            $("#overallAnalysis").hide();
            $("#overallAnalysis").empty();
            $("#faceAnalysis").hide();
            $("#faceAnalysis").empty();
            $("#handMotion").hide();
            $("#handMotion").empty();
            $("#soundAnalysis").hide();
            $("#soundAnalysis").empty();
            $("#feverAnalysis").show();
            $("#twitterAna").hide();
            $("#twitterAna").empty();
            feverChart();
        }
        else if (i == 3) {
            i = i - 1;
            $("#overallAnalysis").hide();
            $("#overallAnalysis").empty();
            $("#faceAnalysis").hide();
            $("#faceAnalysis").empty();
            $("#handMotion").hide();
            $("#handMotion").empty();
            $("#soundAnalysis").show();
            $("#feverAnalysis").hide();
            $("#feverAnalysis").empty();
            $("#twitterAna").hide();
            $("#twitterAna").empty();
            soundChart();
        }
        else if (i == 2) {
            i = i - 1;
            $("#overallAnalysis").hide();
            $("#overallAnalysis").empty();
            $("#faceAnalysis").hide();
            $("#faceAnalysis").empty();
            $("#handMotion").show();
            $("#soundAnalysis").hide();
            $("#soundAnalysis").empty();
            $("#feverAnalysis").hide();
            $("#feverAnalysis").empty();
            $("#twitterAna").hide();
            $("#twitterAna").empty();
            handChart();
        }
        else if (i == 1) {
            i = i - 1;
            $("#overallAnalysis").hide();
            $("#overallAnalysis").empty();
            $("#faceAnalysis").show();
            $("#handMotion").hide();
            $("#handMotion").empty();
            $("#soundAnalysis").hide();
            $("#soundAnalysis").empty();
            $("#feverAnalysis").hide();
            $("#feverAnalysis").empty();
            $("#twitterAna").hide();
            $("#twitterAna").empty();
            faceChart();
        }
        else if (i == 0) {
            i = i - 1;
            $("#overallAnalysis").show();
            $("#faceAnalysis").hide();
            $("#faceAnalysis").empty();
            $("#handMotion").hide();
            $("#handMotion").empty();
            $("#soundAnalysis").hide();
            $("#soundAnalysis").empty();
            $("#feverAnalysis").hide();
            $("#feverAnalysis").empty();
            $("#twitterAna").hide();
            $("#twitterAna").empty();
            overallChart();
        }
        else {
            i = 4;
            $("#overallAnalysis").hide();
            $("#overallAnalysis").empty();
            $("#faceAnalysis").hide();
            $("#faceAnalysis").empty();
            $("#handMotion").hide();
            $("#handMotion").empty();
            $("#soundAnalysis").hide();
            $("#soundAnalysis").empty();
            $("#feverAnalysis").hide();
            $("#feverAnalysis").empty();
            $("#twitterAna").show();
            twitterChart();
        }
    });
});