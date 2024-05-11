import axios from 'axios';
import authHeader from '@/services/dataService';

const API_URL = 'http://localhost:5259/api';

class UserService {
    getMyRole() {
        return axios.get(API_URL + '/auth/roles', { headers: authHeader() });
    }
    async approveActivity(actId){
        const response = await axios.post(
            `${API_URL}/admin/approve/${actId}`,
            {},
            {headers: authHeader()}
        );
        return response.data;
    }
    async declineActivity(actId){
        const response = await axios.post(
            `${API_URL}/admin/decline/${actId}`,
            {},
            {headers: authHeader()}
        );
        return response.data;
    }
    async getAdminActivities(){
        const response = await axios.get(
            `${API_URL}/admin/activities`,
            {headers: authHeader()}
        );
        return response.data;
    }
    async getEventPreview(actId){
        const response = await axios.get(
            `${API_URL}/activity/${actId}/preview`,
            {headers: authHeader()}
        );
        return response.data;
    }
    async getSubscribedActivities(offsetParam, countParam){
        const response = await axios.get(
            `${API_URL}/community/subscribed/activities?offset=${offsetParam}&count=${countParam}`,
            {headers: authHeader()}
        );
    return response.data;
    }
    async getMyModerationEvents(){
        const response = await axios.get(
            `${API_URL}/activity/my/moderation`,
            {headers: authHeader()}
        );
        return response.data;
    }
    async getIsMyActivity(actId){
        const response = await axios.get(
            `${API_URL}/activity/${actId}/my`,
            {headers: authHeader()}
        );
        return response.data;
    }
    async getNotifStatus(){
        const response = await axios.get(
            `${API_URL}/notification/tg`,
            {headers: authHeader()}
        );
        return response.data;
    }
    async enableNotif(){
        const response = await axios.post(
            `${API_URL}/notification/tg`,
            {},
            {headers: authHeader()}
        );
        return response.data;
    }
    async disableNotif(){
        const response = await axios.delete(
            `${API_URL}/notification/tg`,
            {headers: authHeader()}
        );
        return response.data;
    }
    async getTgStatus(){
        const response = await axios.get(
            `${API_URL}/tg/status`,
            {headers: authHeader()}
        );
        return response.data;
    }
    async removeTg(){
        const response = await axios.delete(
            `${API_URL}/tg/remove`,
            {headers: authHeader()}
        );
        return response.data;
    }
    async updateActivitySmallPhoto(payload){
        const id = payload.activityId;
        const formData = new FormData();
        formData.append('file', payload.smallPhoto);

        const response = await axios.put(
            `${API_URL}/activity/${id}/image/small`,
            formData,
            {headers: authHeader()}
        );
        return response.data;
    }
    async updateActivityBannerPhoto(payload){
        const id = payload.activityId;
        const formData = new FormData();
        formData.append('file', payload.bannerPhoto);

        const response = await axios.put(
            `${API_URL}/activity/${id}/image/banner`,
            formData,
            {headers: authHeader()}
        );
        return response.data;
    }
    async startTgConnect(tag){
        const response = await axios.post(
            `${API_URL}/tg/startconnect`,
            {"tag": tag},
            {headers: authHeader()}
        );
        return response.data;
    }

    async updateCommunityPhoto(payload){
        const id = payload.communityId;
        const formData = new FormData();
        formData.append('file', payload.photo);

        const response = await axios.put(
            `${API_URL}/community/${id}/image/small`,
            formData,
            {headers: authHeader()}
        );
        return response.data;
    }
    async updateGroup(payload){
        const id = payload.id;
        delete payload.id;
        const response = await axios.patch(
            `${API_URL}/community/${id}`,
            payload,
            {headers: authHeader()}
        );
        return response.data;
    }
    async removeGroup(commId){
        const response = await axios.delete(
            `${API_URL}/community/${commId}`,
            {headers: authHeader() });
        return response.data;
    }
    async createGroup(payload){
        const response = await axios.post(
            `${API_URL}/community/`,
            payload,
            {headers: authHeader()}
        );
        return response.data;
    }
    async createEvent(payload){
        const response = await axios.post(
            `${API_URL}/activity/`,
            payload,
            {headers: authHeader()}
        );
        return response.data;
    }
    async updateEvent(payload){
        const id = payload.activityId;
        delete payload.id;
        const response = await axios.patch(
            `${API_URL}/activity/${id}`,
            payload,
            {headers: authHeader()}
        );
        return response.data;
    }
    async getUserOwnGroups(){
        const response = await axios.get(
            `${API_URL}/community/my`,
            {headers: authHeader() });
        return response.data;
    }
    async removeEvent(actId){
        const response = await axios.delete(
            `${API_URL}/activity/${actId}`,
            {headers: authHeader() });
        return response.data;
    }
    async getMyOwnEvents(){
        const response = await axios.get(
            `${API_URL}/activity/my`,
            {headers: authHeader() });
        return response.data;
    }
    async getMyOwnEventsPast(){
        const response = await axios.get(
            `${API_URL}/activity/my?past=true`,
            {headers: authHeader() });
        return response.data;
    }
    async isMemberOfEvent(actId){
        const response = await axios.get(
            `${API_URL}/activity/${actId}/incalendar`,
            {headers: authHeader() });
        return response.data;
    }
    async removeFromCalendar(actId){
        const response = await axios.delete(
            `${API_URL}/calendar/${actId}`,
            {headers: authHeader() });
        return response.data;
    }
    async addToCalendar(actId){
        const response = await axios.post(
            `${API_URL}/calendar/${actId}`,
            {},
            {headers: authHeader() });
        return response.data;
    }
    async getMyEventsByMonthYear(monthId, yearId){
        const response = await axios.get(
            `${API_URL}/calendar/year/${yearId}/month/${monthId}`,
            {headers: authHeader() });
        return response.data;
    }
    async getMyEventsByMonthPast(monthId, yearId){
        const response = await axios.get(
            `${API_URL}/calendar/year/${yearId}/month/${monthId}?past=true`,
            {headers: authHeader() });
        return response.data;
    }
    async isSubscriber(commId) {
        const response = await axios.get(
            `${API_URL}/community/${commId}/subscribed`,
            {headers: authHeader() });
        return response.data;
    }
    async getGroupEvents(commId) {
        const response = await axios.get(
            `${API_URL}/community/${commId}/activity`,
            {headers: authHeader() });
        return response.data;
    }
    async getGroupDetails(commId) {
        const response = await axios.get(
            `${API_URL}/community/${commId}`,
            {headers: authHeader() });
        return response.data;
    }
    async getSubscriptions() {
        const response = await axios.get(
            `${API_URL}/community/subscribed`, {headers: authHeader()});
        return response.data;
    }
    async searchEvents(input, offsetParam, countParam) {
        console.log("before post input")
        console.log(input);
        let body =  { cityId: null, categoriesIds: null };
        if (input?.cityId !== "")
            body.cityId = input.cityId;
        if (input?.titleId !== "")
            body.title = input.title;
        if (input?.categoriesIds !== "")
            body.categoriesIds = input.categoriesIds;
        if (input?.tagsIds !== "")
            body.tagsIds = input.tagsIds;
        if (input.searchDates.length > 0)
            body.dateRange = input.searchDates;
        if (input.searchPast === true)
            body.past = true;

        console.log("body before send")
        console.log(body);

        const response = await axios.post(
            `${API_URL}/activity/filter`,
            body,
            {headers: authHeader(), params: {offset: offsetParam, count: countParam}}
        );
        return response.data;
    }
    async getCategoryList() {
        const response = await axios.get(
        `${API_URL}/category`, {headers: authHeader()});
        return response.data;
    }
    async getCityList() {
        const response = await axios.get(
            `${API_URL}/city`,
            {headers: authHeader()}
        );
        return response.data;
    }
    async getTagsList() {
        const response = await axios.get(
            `${API_URL}/tag`,
            {headers: authHeader()}
        );
        return response.data;
    }
    async getEvents(offsetParam, countParam) {
        const response = await axios.get(
            `${API_URL}/activity`,
            {params: {offset: offsetParam, count: countParam}, headers: authHeader()},
        );
        return response.data;
    }
    async getEventDetails(actId) {
        const response = await axios.get(
            `${API_URL}/activity/${actId}`,
            {headers: authHeader() });
        return response.data;
    }
    async subscribeGroup(commId){
        const response = await axios.post(
            `${API_URL}/community/${commId}/subscribe`,
            {},
            {headers: authHeader() });
        return response.data;
    }
    async unsubGroup(groupId) {
        const response = await axios.delete(
            `${API_URL}/community/${groupId}/unsubscribe`,
            {headers: authHeader() });
        return response.data;
    }
}

export default new UserService();