mergeInto(LibraryManager.library, {
    AuthStatus: function () {
        ysdk.feedback.canReview().then(({ value, reason }) => {
            if (value) {
                myGameInstance.SendMessage('Yandex', 'AuthentificationStatus');
            }
        })
    },

    RateGame: function () {
        ysdk.feedback.canReview().then(({ value, reason }) => {
            if (value) {
                ysdk.feedback.requestReview().then(({ feedbackSent }) => {
                    myGameInstance.SendMessage('Yandex', 'OffAuthentificationStatus');
                    console.log(feedbackSent);
                })
            }
            else
            {
                myGameInstance.SendMessage('Yandex', 'OffAuthentificationStatus');
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
            lb.setLeaderboardScore('lbTime', value);
        });
    },

    ShowAdv : function () {
        ysdk.adv.showFullscreenAdv({
            callbacks: {
                onClose: function(wasShown) {
                },
                onError: function(error) {
                    // Действие в случае ошибки.
                }
            }
        })
    },

  });