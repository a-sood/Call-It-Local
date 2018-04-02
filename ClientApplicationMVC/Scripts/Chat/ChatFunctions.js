var currentSelectedChat = null;

/**
*   This function will set the on click functions for the send button and chat instances.
*/
$(function () {//This function is executed after the entire page is loaded
    $("#SendButton").click(sendMessage);
    $("#ChatInstancesList").children().each(function () {
        $(this).click(chatInstanceSelected);
    });
    var firstChatInstanceBox = $("#ChatInstancesList").children().first();

    firstChatInstanceBox.css("background", "rgba(255, 0, 0, 0.1)");
    currentSelectedChat = firstChatInstanceBox.attr("id");

    // Establish Connection to Chat Hub
   connectToChatHub();
    while (myConnection === null || myConnection === 'undefined') {
        connectToChatHub();
    }
});

/**
 *  Validates the message the user is trying to send, and sends it.
*   This function will reset the message box and append the message the user sent to the message display area
 */
function sendMessage() {
    var userData = $("#textUserMessage").val();
    if ($.trim(userData) === "") {
        return;
    }

    //Clear the chat box
    $("#textUserMessage").val("");

    // Add to our chat box
    addTextToChatBox(userData, "You");
    var recipient = currentSelectedChat;
    var timestamp = Math.round((new Date()).getTime() / 1000);

    /** POST TO DATABASE **/
    $.post("/Chat/SendMessage", {
        receiver: recipient,
        timestamp: timestamp,
        message: userData
    });

    /** USE SIGNALR TO UPDATE RECIPIENTS CHAT IF THEY ARE LOOKING AT IT **/
    myConnection.server.sendMessage(recipient, userData);

}

/**
 * This function adds the given text to the user and indicates the sender of the text.
 * @param {string} text - The content of the message
 * @param {string} sender - The username of the sender. If it is "You" it will be a different colour.
 */
function addTextToChatBox(text, sender) {
    var newMessageHtml =
        "<p class='message'>" +
        "<span class='username'";

    if (sender === "You") {
        newMessageHtml += ">You: ";
    }
    else if (currentSelectedChat === sender) {
        newMessageHtml += " style='color:aqua;'>" + sender + ": ";
    }
    else { // Message Thread Not Focused, Show a star to indicate it has a new message
        $("#" + sender + "_text").text(sender + "*");
        return; 
    }

    newMessageHtml += "</span>" + text + "</p>";

    $("#ConversationDisplayArea").html(// Add the new message to the message display area.
        $("#ConversationDisplayArea").html() + newMessageHtml);

    $("#ConversationDisplayArea").scrollTop($("#ConversationDisplayArea").prop("scrollHeight"));//Make the scrollbar scroll to the bottom
}

/**
 * When a user selects their chat history with a specific user, this function will load and display the chat history.
 */
function chatInstanceSelected() {
    if ($(this).attr("id") === currentSelectedChat) {
        return;
    }

    $("#" + currentSelectedChat).css("background", "initial");
    $("#" + currentSelectedChat + "_text").text(currentSelectedChat);

    currentSelectedChat = $(this).attr("id");

    $("#" + currentSelectedChat).css("background", "rgba(255, 0, 0, 0.1)");

    $.ajax({
        method: "GET",
        url: "/Chat/Conversation",
        data: {
            otherUser: currentSelectedChat
        },
        success: function (data) {
            $("#ConversationDisplayArea").html(data);
        }
    });
}
/**
* To connect client to the SignalR chat hub on Server side
*/
function connectToChatHub() {
    // myConnection is a global variable becuase no 'var' prefix
    myConnection = $.connection.hub.createHubProxy("ChatHub");
    
    /** Define update text function called by Chat Hub on Server Side **/
    myConnection.client.addTextToChatBox = function (text, sender) {
        addTextToChatBox(text, sender);
    };

    // Pass the clients username to the ChatHub (the ChatHub cannot access the session state user like our controllers)
    $.connection.hub.qs = { "username": $("#username_title").val() };

    // Update view
    $.connection.hub.start().done(function () {
        $("#status_msg").text("Status: Connected");
        $("#status_msg").css("color", "green");
    });}