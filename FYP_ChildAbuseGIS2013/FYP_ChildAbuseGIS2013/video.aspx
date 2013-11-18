<%@ Page Title="" Language="C#" MasterPageFile="~/Design.Master" AutoEventWireup="true"
    CodeBehind="video.aspx.cs" Inherits="FYP_ChildAbuseGIS2013.video" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <head>
        <link rel="Stylesheet" type="text/css" href="FlowPlayer/videoplayer.css" />
        <script src="FlowPlayer/flowplayer-3.2.12.min.js" type="text/javascript"></script>
        <script src="js/videoplayer.js" type="text/javascript"></script>
        <script>
            (function (d, s, id) {
                var js, fjs = d.getElementsByTagName(s)[0];
                if (d.getElementById(id)) return;
                js = d.createElement(s); js.id = id;
                js.src = "//connect.facebook.net/en_US/all.js#xfbml=1";
                fjs.parentNode.insertBefore(js, fjs);
            } (document, 'script', 'facebook-jssdk'));
        </script>
    </head>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">
    <div id="dvItemTemplate">
        <table>
            <tr>
                <td>
                    <img src="image/thermal.jpg" onclick="dim_div()" />
                    <div id="toggle_div">
                        <button id="button" onclick="dim_div_undim()" style="float: right">
                            <img src="image/close.png" /></button>
                        <a class="myPlayer" href="videos/thermal.mp4" style="background-image: url(image/white.jpg)">
                            <img src="image/play.jpg" alt="Demo video 1" />
                        </a>
                        <div class="fb-comments" data-href="http://example.com/comments" data-numposts="5"
                            style="position: absolute; left: 31%; bottom: 15px; z-index: 999; background-color: white">
                        </div>
                    </div>
                </td>
                <td>
                    <img src="image/c.JPG" onclick="dim_div1()" />
                    <div id="toggle_div1">
                        <button id="button1" onclick="dim_div1_undim()" style="float: right">
                            <img src="image/close.png" /></button>
                        <a class="myPlayer" href="videos/c.mp4" style="background-image: url(image/white.jpg);
                            margin-left: auto; margin-right: auto">
                            <img src="image/play.jpg" alt="Demo video 2" />
                        </a>
                        <div class="fb-comments" data-href="http://example.com/comments" data-numposts="5"
                            style="position: absolute; left: 31%; bottom: 15px; z-index: 999; background-color: white">
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <img src="image/My_Movie.JPG" onclick="dim_div2()" />
                    <div id="toggle_div2">
                        <button id="button2" onclick="dim_div2_undim()" style="float: right">
                            <img src="image/close.png" /></button>
                        <a class="myPlayer" href="videos/My_Movie.mp4" style="background-image: url(image/white.jpg)">
                            <img src="image/play.jpg" alt="Demo video 3" />
                        </a>
                        <div class="fb-comments" data-href="http://example.com/comments" data-numposts="5"
                            style="position: absolute; left: 31%; bottom: 15px; z-index: 999; background-color: white">
                        </div>
                    </div>
                </td>
                <td>
                    <img src="image/My_Movie2.JPG" onclick="dim_div3()" />
                    <div id="toggle_div3">
                        <button id="button3" onclick="dim_div3_undim()" style="float: right">
                            <img src="image/close.png" /></button>
                        <a class="myPlayer" href="videos/My_Movie2.flv" style="background-image: url(image/white.jpg)">
                            <img src="image/play.jpg" alt="Demo video 3" />
                        </a>
                        <div class="fb-comments" data-href="http://example.com/comments" data-numposts="5"
                            style="position: absolute; left: 31%; bottom: 15px; z-index: 999; background-color: white">
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <img src="image/Part 1.JPG" onclick="dim_div4()" />
                    <div id="toggle_div4">
                        <button id="button4" onclick="dim_div4_undim()" style="float: right">
                            <img src="image/close.png" /></button>
                        <a class="myPlayer" href="videos/Part 1.flv" style="background-image: url(image/white.jpg)">
                            <img src="image/play.jpg" alt="Demo video 3" />
                        </a>
                        <div class="fb-comments" data-href="http://example.com/comments" data-numposts="5"
                            style="position: absolute; left: 31%; bottom: 15px; z-index: 999; background-color: white">
                        </div>
                    </div>
                </td>
                <td>
                    <img src="image/Part 2.JPG" onclick="dim_div5()" />
                    <div id="toggle_div5">
                        <button id="button5" onclick="dim_div5_undim()" style="float: right">
                            <img src="image/close.png" /></button>
                        <a class="myPlayer" href="videos/Part 2.flv" style="background-image: url(image/white.jpg)">
                            <img src="image/play.jpg" alt="Demo video 3" />
                        </a>
                        <div class="fb-comments" data-href="http://example.com/comments" data-numposts="5"
                            style="position: absolute; left: 31%; bottom: 15px; z-index: 999; background-color: white">
                        </div>
                    </div>
                    <br clear="all" />
                </td>
            </tr>
        </table>
        <br clear="all" />
    </div>
    <div id="twitter" style="float: right; position: fixed; top: 30px; right: 450px;">
        <a class="twitter-timeline" href="https://twitter.com/fyp_CAanalysis" data-widget-id="399710961268826112">
            Tweets by @fyp_CAanalysis</a>
    </div>
    <div id="fb-root">
    </div>
    </form>
</asp:Content>
