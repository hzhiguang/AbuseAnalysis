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

function faceChart(){
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
    var i = 0;
    $("#next").css("opacity", "0.4");
    $("#prev").css("opacity", "0.4");
    $("#faceAnalysis").hide();
    $("#handMotion").hide();
    $("#twitterAna").hide();

    $("#next").mouseover(function () {
        $(this).css("opacity", "1");
    }).mouseout(function () {
        $(this).css("opacity", "0.4");
    }).click(function () {
        if (i == 0) {
            i++;
            $("#faceAnalysis").show();
            $("#handMotion").hide();
            $("#handMotion").empty();
            $("#twitterAna").hide();
            $("#twitterAna").empty();
            faceChart();
        }
        else if (i == 1) {
            i++;
            $("#faceAnalysis").hide();
            $("#faceAnalysis").empty();
            $("#handMotion").show();
            $("#twitterAna").hide();
            $("#twitterAna").empty();
            handChart();
        }
        else if (i == 2) {
            i++;
            $("#faceAnalysis").hide();
            $("#faceAnalysis").empty();
            $("#handMotion").hide();
            $("#handMotion").empty();
            $("#twitterAna").show();
            twitterChart();
        }
        else {
            i = 1;
            $("#faceAnalysis").show();
            $("#handMotion").hide();
            $("#handMotion").empty();
            $("#twitterAna").hide();
            $("#twitterAna").empty();
            faceChart();
        }
    });

    $("#prev").mouseover(function () {
        $(this).css("opacity", "1");
    }).mouseout(function () {
        $(this).css("opacity", "0.4");
    }).click(function () {
        if (i == 2) {
            i--;
            $("#faceAnalysis").hide();
            $("#faceAnalysis").empty();
            $("#handMotion").hide();
            $("#handMotion").empty();
            $("#twitterAna").show();
            twitterChart();
        }
        else if (i == 1) {
            i--;
            $("#faceAnalysis").hide();
            $("#faceAnalysis").empty();
            $("#handMotion").show();
            $("#twitterAna").hide();
            $("#twitterAna").empty();
            handChart();
        }
        else if (i == 0) {
            i--;
            $("#faceAnalysis").show();
            $("#handMotion").hide();
            $("#handMotion").empty();
            $("#twitterAna").hide();
            $("#twitterAna").empty();
            faceChart();
        }
        else {
            i = 1;
            $("#faceAnalysis").hide();
            $("#faceAnalysis").empty();
            $("#handMotion").hide();
            $("#handMotion").empty();
            $("#twitterAna").show();
            twitterChart();
        }
    });

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
        }
    }
    http_request.open("GET", data_file, true);
    http_request.send();
});