<template>
  <div class="user-avator-dropdown">
    <Dropdown @on-click="handleClick">
      <Badge :dot="false">
        <Avatar :src="userAvator" />
        <span>{{ this.$store.state.user.userName}}</span>
      </Badge>
      <Icon :size="18" type="md-arrow-dropdown"></Icon>
      <DropdownMenu slot="list">
        <!-- <DropdownItem name="message">
          消息中心<Badge style="margin-left: 10px" :count="messageUnreadCount"></Badge>
        </DropdownItem>-->
        <DropdownItem name="updatePWdModel">
          修改密码
          <Badge style="margin-left: 10px"></Badge>
        </DropdownItem>
        <DropdownItem name="logout">退出登录</DropdownItem>
      </DropdownMenu>
    </Dropdown>
    <!--  修改密码弹出框-->
    <Modal title="修改密码" v-model="updatePwd" class-name="vertical-center-modal" footer-hide>
      <Form ref="formInline" :model="formInline" :rules="ruleInline">
        <Row>
          <Col span="24">
            <FormItem label="原密码" prop="oldpwd">
              <Input
                type="password"
                v-model="formInline.oldpwd"
                placeholder="请输入原密码"
                :maxlength="20"
              />
            </FormItem>
          </Col>
        </Row>
        <Row>
          <Col span="24">
            <FormItem label="新密码" prop="newpwd">
              <Input
                type="password"
                v-model="formInline.newpwd"
                placeholder="请输入新密码"
                :maxlength="20"
              />
            </FormItem>
          </Col>
        </Row>
        <Row>
          <Col span="24">
            <FormItem label="确认密码" prop="newpwd2">
              <Input
                type="password"
                v-model="formInline.newpwd2"
                placeholder="请再次输入新密码"
                :maxlength="20"
              />
            </FormItem>
          </Col>
        </Row>
        <FormItem>
          <Button type="primary" @click="handleSubmit()">确认修改</Button>
        </FormItem>
      </Form>
    </Modal>
  </div>
</template>

<script>
import "./user.less";
import { mapActions } from "vuex";
import { editUserPwd } from "@/api/rbac/user";
import { getWarn } from "@/api/base/wingWarn";
export default {
  name: "User",
  props: {
    userAvator: {
      type: String,
      default: ""
    },
    messageUnreadCount: {
      type: Number,
      default: 0
    }
  },
  data() {
    return {
      messageCount: 0,
      updatePwd: false,
      formInline: {
        oldpwd: "",
        newpwd: "",
        newpwd2: ""
      },
      ruleInline: {
        oldpwd: [{ required: true, message: "密码不能为空" }],
        newpwd: [{ required: true, message: "新密码不能为空" }],
        newpwd2: [{ required: true, message: "确认密码不能为空" }]
      }
    };
  },
  methods: {
    ...mapActions(["handleLogOut"]),
    logout() {
      var $this = this;
      this.$Modal.confirm({
        title: "退出确认",
        content: "确定要退出系统吗?",
        okText: "确定退出",
        cancelText: "再想想",
        loading: true,
        onOk() {
          setTimeout(function() {
            $this.handleLogOut().then(() => {
              $this.$Modal.remove();
              $this.$router.push({
                name: "login"
              });
            });
          }, 1500);
        }
      });
    },
    message() {
      this.$router.push({
        name: "message_page"
      });
    },
    handleClick(name) {
      switch (name) {
        case "logout":
          this.logout();
          break;
        case "message":
          this.message();
          break;
        case "updatePWdModel":
          this.updatePWdModel();
          break;
      }
    },
    updatePWdModel() {
      var storage = window.localStorage;
      this.username = storage["username"];
      this.updatePwd = true;
    },
    validateConsumableForm() {
      let _valid = false;
      this.$refs["formInline"].validate(valid => {
        if (!valid) {
          this.$Message.error("请完善表单信息");
        } else {
          _valid = true;
        }
      });
      return _valid;
    },
    // 修改密码
    handleSubmit() {
      var $this = this;
      let valid = this.validateConsumableForm();
      if (valid) {
        if (this.formInline.newpwd.trim() != this.formInline.newpwd2.trim()) {
          this.$Message.error("两次密码不一致，请重新输入!");
          return;
        }
        editUserPwd(this.formInline).then(res => {
          if (res.data.code === 200) {
            this.$Message.success(res.data.message);
            //关闭修改对话框
            this.updatePwd = false;
            //清空数据
            this.$refs.formInline.resetFields();
            //返回登录界面
            setTimeout(function() {
              $this.handleLogOut().then(() => {
                $this.$Modal.remove();
                $this.$router.push({
                  name: "login"
                });
              });
            }, 1500);
          } else {
            this.$Message.warning(res.data.message);
          }
        });
      }
    }
  },
  mounted() {
    getWarn().then(res => {
      ////console.log(res.data.data);
      // if (res.data.totalCount > 0) {
      //   this.$Notice.warning({
      //     name: "warn",
      //     title: "警告",
      //     desc: "<a href='#/base/wingWarning'>今天还有" + res.data.totalCount + "个垃圾箱房打卡超时</a>",
      //     duration: 0
      //   });
      // }
    });

    ////console.log(this.$Notice);
  }
};
</script>
