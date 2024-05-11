import UserService from '@/services/userService';



export const user = {
    namespaced: true,
    state: {},
    actions: {
        getMyRole({commit}){
            return UserService.getMyRole().then(
                data => {
                    return Promise.resolve(data);
                },
                error => {
                    return Promise.reject(error);
                }
            )
        },
        approveActivity({commit}, actId){
            return UserService.approveActivity(actId).then(
                data => {
                    return Promise.resolve(data);
                },
                error => {
                    return Promise.reject(error);
                }
            )
        },
        declineActivity({commit}, actId){
            return UserService.declineActivity(actId).then(
                data => {
                    return Promise.resolve(data);
                },
                error => {
                    return Promise.reject(error);
                }
            )
        },
        getAdminActivities({commit}){
            return UserService.getAdminActivities().then(
                data => {
                    return Promise.resolve(data);
                },
                error => {
                    return Promise.reject(error);
                }
            )
        },
        getEventPreview({commit}, actId){
            return UserService.getEventPreview(actId).then(
                data => {
                    return Promise.resolve(data);
                },
                error => {
                    return Promise.reject(error);
                }
            )
        },
        getMyModerationEvents({commit}){
            return UserService.getMyModerationEvents().then(
                data => {
                    return Promise.resolve(data);
                },
                error => {
                    return Promise.reject(error);
                }
            )
        },
        getSubscribedActivities({commit}, params){
            return UserService.getSubscribedActivities(params.offsetParam, params.countParam).then(
                data => {
                    return Promise.resolve(data);
                },
                error => {
                    return Promise.reject(error);
                }
            )
        },
        getIsMyActivity({commit}, actId){
            return UserService.getIsMyActivity(actId).then(
                data => {
                    return Promise.resolve(data);
                },
                error => {
                    return Promise.reject(error);
                }
            )
        },
        getNotifStatus({commit}){
            return UserService.getNotifStatus().then(
                data => {
                    return Promise.resolve(data);
                },
                error => {
                    return Promise.reject(error);
                }
            )
        },
        enableNotif({commit}){
            return UserService.enableNotif().then(
                data => {
                    return Promise.resolve(data);
                },
                error => {
                    return Promise.reject(error);
                }
            )
        },
        disableNotif({commit}){
            return UserService.disableNotif().then(
                data => {
                    return Promise.resolve(data);
                },
                error => {
                    return Promise.reject(error);
                }
            )
        },
        removeTg({commit}){
            return UserService.removeTg().then(
                data => {
                    return Promise.resolve(data);
                },
                error => {
                    return Promise.reject(error);
                }
            )
        },
        getTgStatus({commit}){
            return UserService.getTgStatus().then(
                data => {
                    return Promise.resolve(data);
                },
                error => {
                    return Promise.reject(error);
                }
            )
        },
        updateGroup({commit}, payload){
            return UserService.updateGroup(payload).then(
                data => {
                    return Promise.resolve(data);
                },
                error => {
                    return Promise.reject(error);
                }
            )
        },
        removeGroup({commit}, groupId){
            return UserService.removeGroup(groupId).then(
                data => {
                    return Promise.resolve(data);
                },
                error => {
                    return Promise.reject(error);
                }
            )
        },
        createGroup({commit}, payload){
            return UserService.createGroup(payload).then(
                data => {
                    return Promise.resolve(data);
                },
                error => {
                    return Promise.reject(error);
                }
            )
        },
        createEvent({commit}, payload){
            return UserService.createEvent(payload).then(
                data => {
                    return Promise.resolve(data);
                },
                error => {
                    return Promise.reject(error);
                }
            )
        },
        updateEvent({commit}, payload){
            return UserService.updateEvent(payload).then(
                data => {
                    return Promise.resolve(data);
                },
                error => {
                    return Promise.reject(error);
                }
            )
        },
        updateActivitySmallPhoto({commit}, payload){
            return UserService.updateActivitySmallPhoto(payload).then(
                data => {
                    return Promise.resolve(data);
                },
                error => {
                    return Promise.reject(error);
                }
            )
        },
        startTgConnect({commit}, tag){
            return UserService.startTgConnect(tag).then(
                data => {
                    return Promise.resolve(data);
                },
                error => {
                    return Promise.reject(error);
                }
            )
        },
        updateActivityBannerPhoto({commit}, payload){
            return UserService.updateActivityBannerPhoto(payload).then(
                data => {
                    return Promise.resolve(data);
                },
                error => {
                    return Promise.reject(error);
                }
            )
        },
        updateCommunityPhoto({commit}, payload){
            return UserService.updateCommunityPhoto(payload).then(
                data => {
                    return Promise.resolve(data);
                },
                error => {
                    return Promise.reject(error);
                }
            )
        },
        getUserOwnGroups({commit}){
            return UserService.getUserOwnGroups().then(
                data => {
                    return Promise.resolve(data);
                },
                error => {
                    return Promise.reject(error);
                }
            )
        },
        removeEvent({commit}, eventId){
            return UserService.removeEvent(eventId).then(
                data => {
                    return Promise.resolve(data);
                },
                error => {
                    return Promise.reject(error);
                }
            )
        },
        getMyOwnEvents({commit}){
          return UserService.getMyOwnEvents().then(
              data => {
                  return Promise.resolve(data);
              },
              error => {
                  return Promise.reject(error);
              }
          )
        },
        getMyOwnEventsPast({commit}){
            return UserService.getMyOwnEventsPast().then(
                data => {
                    return Promise.resolve(data);
                },
                error => {
                    return Promise.reject(error);
                }
            )
        },

        isMemberOfEvent({commit}, eventId){
            return UserService.isMemberOfEvent(eventId).then(
                data => {
                    return Promise.resolve(data);
                },
                error => {
                    return Promise.reject(error);
                }
            )
        },
        addToCalendar({commit}, eventId){
            return UserService.addToCalendar(eventId).then(
                data => {
                    return Promise.resolve(data);
                },
                error => {
                    return Promise.reject(error);
                }
            )
        },
        removeFromCalendar({commit}, eventId){
          return UserService.removeFromCalendar(eventId).then(
              data => {
                  return Promise.resolve(data);
              },
              error => {
                  return Promise.reject(error);
              }
          )
        },
        getMyEventsByMonth({commit}, date){
            return UserService.getMyEventsByMonthYear(date.month, date.year).then(
                data => {
                    return Promise.resolve(data);
                },
                error => {
                    return Promise.reject(error);
                }
            )
        },
        getMyEventsByMonthPast({commit}, date){
            return UserService.getMyEventsByMonthPast(date.month, date.year).then(
                data => {
                    return Promise.resolve(data);
                },
                error => {
                    return Promise.reject(error);
                }
            )
        },
        isSubscriber({commit}, groupId){
           return UserService.isSubscriber(groupId).then(
               data => {
                   return Promise.resolve(data);
               },
               error => {
                   return Promise.reject(error);
               }
           )
        }, 
        getGroupEvents({commit}, groupId) {
          return UserService.getGroupEvents(groupId).then(
              data => {
                  return Promise.resolve(data);
              },
              error => {
                  return Promise.reject(error);
              }
          )
        },
        getGroupDetails({commit}, groupId){
            return UserService.getGroupDetails(groupId).then(
                data => {
                    return Promise.resolve(data);
                },
                error => {
                    return Promise.reject(error);
                }
            )
        },
        getSubscriptions({commit}){
            return UserService.getSubscriptions().then(
                data => {
                    return Promise.resolve(data);
                },
                error => {
                    return Promise.reject(error);
                }
            )
        },
        searchEvents({commit}, params){
         return UserService.searchEvents(params.input, params.offsetParam, params.countParam).then(
             data => {
                 return Promise.resolve(data);
             },
             error => {
                 return Promise.reject(error);
             }
         )
        },
        getCategoryList({commit}) {
          return UserService.getCategoryList().then(
              data => {
                  return Promise.resolve(data);
              },
              error => {
                  return Promise.reject(data);
              }
          )
        },
        getCityList({commit}) {
          return UserService.getCityList().then(
              data => {
                  return Promise.resolve(data);
              },
              error => {
                  return Promise.reject(data);
              }
          )
        },
        getTagsList({commit}) {
            return UserService.getTagsList().then(
                data => {
                    return Promise.resolve(data);
                },
                error => {
                    return Promise.reject(error);
                }
            );
        },
        getEvents({commit}, params) {
          return UserService.getEvents(params.offsetParam, params.countParam).then(
              data => {
                  return Promise.resolve(data);
              },
              error => {
                  return Promise.reject(error);
              }
          )
        },
        getEventDetails({commit}, eventId) {
            return UserService.getEventDetails(eventId).then(
                data => {
                    return Promise.resolve(data);
                },
                error => {
                    return Promise.reject(error);
                }
            )
        },
        subscribe({commit}, groupId) {
            return UserService.subscribeGroup(groupId).then(
                data => {
                    return Promise.resolve(data);
                },
                error => {
                    return Promise.reject(error);
                }
            );
        },
        unsubscribe({commit}, groupId) {
          return UserService.unsubGroup(groupId).then(
              data => {
                  return Promise.resolve(data);
              },
              error => {
                  return Promise.reject(error);
              }
          );
        },
    },
    mutations: {

    }
};
