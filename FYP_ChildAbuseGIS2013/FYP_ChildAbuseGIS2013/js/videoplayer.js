function dim_div() {
    document.getElementById("toggle_div").style.display = "block";
}

function dim_div1() {
    document.getElementById("toggle_div1").style.display = "block";
}

function dim_div2() {
    document.getElementById("toggle_div2").style.display = "block";
}

function dim_div3() {
    document.getElementById("toggle_div3").style.display = "block";
}

function dim_div4() {
    document.getElementById("toggle_div4").style.display = "block";
}

function dim_div5() {
    document.getElementById("toggle_div5").style.display = "block";
}

function dim_div_undim() {
    document.getElementById("toggle_div_undim").style.display = "block";
}

function dim_div1_undim() {
    document.getElementById("toggle_div_undim").style.display = "block";
}

function dim_div2_undim() {
    document.getElementById("toggle_div_undim").style.display = "block";
}

function dim_div3_undim() {
    document.getElementById("toggle_div_undim").style.display = "block";
}

function dim_div4_undim() {
    document.getElementById("toggle_div_undim").style.display = "block";
}

function dim_div5_undim() {
    document.getElementById("toggle_div_undim").style.display = "block";
}

!function (d, s, id) { var js, fjs = d.getElementsByTagName(s)[0], p = /^http:/.test(d.location) ? 'http' : 'https'; if (!d.getElementById(id)) { js = d.createElement(s); js.id = id; js.src = p + "://platform.twitter.com/widgets.js"; fjs.parentNode.insertBefore(js, fjs); } } (document, "script", "twitter-wjs");

flowplayer("a.myPlayer", "FlowPlayer/flowplayer-3.2.16.swf", {
    // this is the player configuration. You'll learn on upcoming demos.

    plugins: {
        clip: {
            onMouseOver: function () {
                // a handle to the player container
                var container = this.getParent();

                // make it to a jQuery object and use the expose tool with a custom color
                $(container).expose({ color: "#254558", api: true });
            },
            // close the expose when the mouse pointer moves out
            onMouseOut: function () {
                $.mask.close();
            }
        },
        share: {
            url: "FlowPlayer/flowplayer.sharing-3.2.14.swf",
            buttons: {
                overColor: 'rgba(255,10,10,0.5)'
            },
            // this demo relies heavily on events provided by the api
            // therefore disable embedding without api and video sharing on facebook
            embed: false,
            facebook: true
        },

        controls: {
            volume: true

        }
    }
});