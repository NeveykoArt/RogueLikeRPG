mergeInto(LibraryManager.library, {


    Hello: function () {
        window.alert("Hello, world!");
        console.log("Hello, world!");
    },


    GiveMePlayerData: function () {
        myGameInstance.SendMessage('Yandex', 'SetName', player.getName());
        myGameInstance.SendMessage('Yandex', 'SetPhoto', player.getPhoto("medium"));
    },


    RateGame: function () {
        ysdk.feedback.canReview().then(({ value, reason }) => {
            if (value) {
                ysdk.feedback.requestReview().then(({ feedbackSent }) => {
                console.log(feedbackSent);
                })
            }
            else
            {
                console.log(reason)
            }
        })
    },

    SaveExtern: function(date) {
        var dateString = UTF8ToString(date);
        var myobj = JSON.parse(dateString);
        player.setData(myobj);
    },


    LoadExtern: function() {
        player.getData().then(_date => {
            const myJSON = JSON.stringify(_date);
            myGameInstance.SendMessage('SaveProgress', 'SetPlayerInfo', myJSON);
        });
    },

    SetToPointsLeaderboard : function(value) {
        ysdk.getLeaderboards().then(lb => {
            lb.setLeaderboardScore('lbPoints', value);
        });
    },

    SetToTimeLeaderboard : function(value) {
        ysdk.getLeaderboards().then(lb => {
            lb.setLeaderboardScore('Height', value);
        });
    },

    ShowAdv : function () {
        ysdk.adv.showFullscreenAdv({
            callbacks: {
                onClose: function(wasShown) {
		    concole.log("---------ABOBA----------");
                    // Действие после закрытия рекламы.
                },
                onError: function(error) {
                    // Действие в случае ошибки.
                }
            }
        })
    },

  });