'use strict';

angular.module('followingList', ['ngRoute'])
  .component('followingList', {
    templateUrl: 'following/following.html',
    controller: ['$http', '$rootScope', function TweetListController($http, $rootScope) {
      var self = this;

      const requestOptions = {
          headers: { 'X-session': $rootScope.x_session, "Content-type" : "application/json;charset=utf-8"} //taught by 600611001
      };

      $http.get('http://localhost:8080/twitterapi/following/', requestOptions).then(function (response) {
        self.followings = response.data;
      });
      
      self.SendFollowing = SendFollowing(followingname){
        const data = "followingname=" + encodeURIComponent(followingname);
        $http.post('http://localhost:8080/twitterapi/following', data, requestOptions).then(function(response){ });
      }
    }]
});