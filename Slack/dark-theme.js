// add this snippet to file "C:\Users\UserName\AppData\Local\slack\app-X.X.X\resources\app.asar.unpacked\src\static\ssb-interop.js"
// original source - laCour GitHub: https://github.com/laCour/slack-night-mode/issues/73#issuecomment-287467332

document.addEventListener('DOMContentLoaded', function() {
 $.ajax({
   url: 'https://raw.githubusercontent.com/mfcallahan/adhoc/master/Slack/black.css',
   success: function(css) {
     $("<style></style>").appendTo('head').html(css);
   }
 });
});